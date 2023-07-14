using Sandbox;
using System.ComponentModel;
using FPSGame.Weapons;
using System.Collections.Generic;
using FPSGame.Jobs;
using FPSGame.Items;
using System;

namespace FPSGame
{


	
	public partial class FPSPlayer : AnimatedEntity
{

	[Net, Predicted] public Weapon ActiveWeapon { get; set; }
	[ClientInput] public Vector3 InputDirection { get; set; }
	[ClientInput] public Angles ViewAngles { get; set; }

	public DamageInfo LastDamage;
		/// <summary>
		/// Position a player should be looking from in world space.
		/// </summary>
		[Browsable( false )]
	public Vector3 EyePosition
	{
		get => Transform.PointToWorld( EyeLocalPosition );
		set => EyeLocalPosition = Transform.PointToLocal( value );
	}

	/// <summary>
	/// Position a player should be looking from in local to the entity coordinates.
	/// </summary>
	[Net, Predicted, Browsable( false )]
	public Vector3 EyeLocalPosition { get; set; }

	/// <summary>
	/// Rotation of the entity's "eyes", i.e. rotation for the camera when this entity is used as the view entity.
	/// </summary>
	[Browsable( false )]
	public Rotation EyeRotation
	{
		get => Transform.RotationToWorld( EyeLocalRotation );
		set => EyeLocalRotation = Transform.RotationToLocal( value );
	}

	/// <summary>
	/// Rotation of the entity's "eyes", i.e. rotation for the camera when this entity is used as the view entity. In local to the entity coordinates.
	/// </summary>
	[Net, Predicted, Browsable( false )]
	public Rotation EyeLocalRotation { get; set; }

	public BBox Hull
	{
		get => new
		(
			new Vector3( -16, -16, 0 ),
			new Vector3( 16, 16, 64 )
		);
	}

	[BindComponent] public PlayerController Controller { get; }
	[BindComponent] public PlayerAnimator Animator { get; }

	public override Ray AimRay => new Ray( EyePosition, EyeRotation.Forward );


	public FPSPlayer( )
		{
			
			Ammo = new List<int>();
		}
		




		/// <summary>
		/// Called when the entity is first created 
		/// </summary>
		public override void Spawn()
	{
		
		

		base.Spawn();
		Tags.Add( "player" );


		Setjob( new Citizen());
		Money = 13239;

	}

	public void SetActiveWeapon( Weapon weapon )
	{
		ActiveWeapon?.OnHolster();
		ActiveWeapon = weapon;
		ActiveWeapon.OnEquip( this );
	}


		public virtual void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			EnableAllCollisions = true;
			Game.AssertServer();
			Components.Create<PlayerController>();
			Components.Create<PlayerAnimator>();
			LifeState = LifeState.Alive;
			Velocity = Vector3.Zero;
			this.ClearWaterLevel();
			
			CreateHull();

			GameManager.Current?.MoveToSpawnpoint( this );
			ResetInterpolation();

			SetActiveWeapon( new Fists() );
			GiveAmmo( AmmoType.Pistol, 100 );
			Tags.Add( "player" );

			Health = 100;
			Armor = 50;
			Hunger = 100;
		}
	



	public void DressFromClient( IClient cl )
	{
		var c = new ClothingContainer();
		c.LoadFromClient( cl );
		c.DressEntity( this );
	}



		TimeSince timeSinceDied;
		public override void Simulate( IClient cl )
	{
		SimulateRotation();
		Controller?.Simulate( cl );
		Animator?.Simulate();
		ActiveWeapon?.Simulate( cl );
		EyeLocalPosition = Vector3.Up * (64f * Scale);



			if ( LifeState == LifeState.Dead )
			{
				if ( timeSinceDied > 3 && Game.IsServer )
				{
					Respawn();
				}

				return;
			}

		}


		public override void OnKilled()
	{

			//GameManager.Current?.OnKilled( this );

		timeSinceDied = 0;
		LifeState = LifeState.Dead;

		//var weapon = ActiveWeapon as Weapon;
		//weapon.Delete();
		EnableAllCollisions = false;
		EnableDrawing = false;
		//CameraMode = new DeathCamera();
		base.OnKilled();
		BecomeRagdollOnClient( LastDamage.Force, LastDamage.BoneIndex );

		}
		protected virtual void CreateHull()
	{
		SetupPhysicsFromAABB( PhysicsMotionType.Keyframed, new Vector3( -16f, -16f, 0f ), new Vector3( 16f, 16f, 72f ) );
		EnableHitboxes = true;
	}

	public override void TakeDamage( DamageInfo info )
	{

			LastDamage = info;
		if ( info.Hitbox.HasTag( "head" ) )
		{
			info.Damage *= 2f;
		}

	

		if ( LifeState == LifeState.Alive )
		{
			base.TakeDamage( info );

			this.ProceduralHitReaction( info );
		}
	}


	public override void BuildInput()
	{
		InputDirection = Input.AnalogMove;

		if ( Input.StopProcessing )
			return;

		var look = Input.AnalogLook;

		if ( ViewAngles.pitch > 90f || ViewAngles.pitch < -90f )
		{
			look = look.WithYaw( look.yaw * -1f );
		}

		var viewAngles = ViewAngles;
		viewAngles += look;
		viewAngles.pitch = viewAngles.pitch.Clamp( -89f, 89f );
		viewAngles.roll = 0f;
		ViewAngles = viewAngles.Normal;
	}

	bool IsThirdPerson { get; set; } = false;
		void ShootEnt()
		{
			var ent = new MoneyPrinter
			{
				Position = EyePosition + EyeRotation.Forward * 50,
				Rotation = EyeRotation

			};

		}

		public virtual void DoFallDamage( TimeSince timeSinceFalling, Vector3 velocity )
		{
			if ( velocity.z > -600 ) return;

			timeSinceFalling /= 2;
			velocity /= 12;
			var damageInfo = new DamageInfo()
			{
				Damage = Math.Abs( velocity.z ) * (1 + timeSinceFalling),
				Force = velocity
			};

			TakeDamage( damageInfo );
		}

		public override void FrameSimulate( IClient cl )
	{
		SimulateRotation();

		Camera.Rotation = ViewAngles.ToRotation();
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );

		if ( Input.Pressed( "view" ) )
		{
				ShootEnt();
		}

		if ( IsThirdPerson )
		{
			Vector3 targetPos;
			var pos = Position + Vector3.Up * 64;
			var rot = Camera.Rotation * Rotation.FromAxis( Vector3.Up, -16 );

			float distance = 80.0f * Scale;
			targetPos = pos + rot.Right * ((CollisionBounds.Mins.x + 50) * Scale);
			targetPos += rot.Forward * -distance;

			var tr = Trace.Ray( pos, targetPos )
				.WithAnyTags( "solid" )
				.Ignore( this )
				.Radius( 8 )
				.Run();
			
			Camera.FirstPersonViewer = null;
			Camera.Position = tr.EndPosition;
		}
		else
		{
			Camera.FirstPersonViewer = this;
			Camera.Position = EyePosition;
		}
	}

	public TraceResult TraceBBox( Vector3 start, Vector3 end, float liftFeet = 0.0f )
	{
		return TraceBBox( start, end, Hull.Mins, Hull.Maxs, liftFeet );
	}

	public TraceResult TraceBBox( Vector3 start, Vector3 end, Vector3 mins, Vector3 maxs, float liftFeet = 0.0f )
	{
		if ( liftFeet > 0 )
		{
			start += Vector3.Up * liftFeet;
			maxs = maxs.WithZ( maxs.z - liftFeet );
		}

		var tr = Trace.Ray( start, end )
					.Size( mins, maxs )
					.WithAnyTags( "solid", "playerclip", "passbullets" )
					.Ignore( this )
					.Run();

		return tr;
	}

	protected void SimulateRotation()
	{
		EyeRotation = ViewAngles.ToRotation();
		Rotation = ViewAngles.WithPitch( 0f ).ToRotation();
	}
}
}
