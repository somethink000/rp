using Sandbox;


namespace MyGame;

public partial class Chat
{
	[ClientRpc]
	public static void AddChatEntry( string name, string message, long playerId = 0, bool isInfo = false )
	{
		Current?.AddEntry( name, message, playerId, isInfo );

		// Only log clientside if we're not the listen server host
		if ( !Game.IsListenServer )
		{
			Log.Info( $"{name}: {message}" ); 
		}
	}

	[ConCmd.Server( "say_somethink" )]
	public static void Say( string message )
	{
		if ( !ConsoleSystem.Caller.IsValid() ) return;

		// todo - reject more stuff
		if ( message.Contains( '\n' ) || message.Contains( '\r' ) )
			return;

		Log.Info( $"{ConsoleSystem.Caller}: {message}" );
		AddChatEntry( To.Everyone, ConsoleSystem.Caller.Name, message, ConsoleSystem.Caller.SteamId );
	}
}
