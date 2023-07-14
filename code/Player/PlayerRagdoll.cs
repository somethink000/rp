﻿using Sandbox;


namespace FPSGame { 
partial class FPSPlayer
{
	// Can create lag, be cautious setting this too high
	static EntityLimit RagdollLimit = new EntityLimit { MaxTotal = 10 };


	[ClientRpc]
	void BecomeRagdollOnClient( Vector3 force, int forceBone )
	{
		var ent = new ModelEntity();
		ent.Position = Position;
		ent.Rotation = Rotation;
		ent.PhysicsEnabled = true;
		ent.UsePhysicsCollision = true;
		Tags.Add( "debris" );

		ent.SetModel( GetModelName() );
		ent.CopyBonesFrom( this );
		ent.TakeDecalsFrom( this );
		ent.SetRagdollVelocityFrom( this );
		ent.DeleteAsync( 20.0f );

		// Copy the clothes over
		foreach ( var child in Children )
		{
			if ( child is ModelEntity e )
			{
				var model = e.GetModelName();
				if ( model != null && !model.Contains( "clothes" ) )
					continue;

				var clothing = new ModelEntity();
				clothing.SetModel( model );
				clothing.SetParent( ent, true );
			}
		}

		ent.PhysicsGroup.AddVelocity( force );

		if ( forceBone >= 0 )
		{
			var body = ent.GetBonePhysicsBody( forceBone );
			if ( body != null )
			{
				body.ApplyForce( force * 1000 );
			}
			else
			{
				ent.PhysicsGroup.AddVelocity( force );
			}
		}

		RagdollLimit.Watch( ent );
	}
}
}
