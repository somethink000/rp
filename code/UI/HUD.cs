using Sandbox;
using Sandbox.UI;

namespace MyGame;

[Library]
public partial class HUD : HudEntity<RootPanel>
{
	public HUD()
	{
		if ( !Game.IsClient )
			return;

		RootPanel.StyleSheet.Load( "/UI/Styles/Style.scss" );


		RootPanel.AddChild<GameChat>();
		RootPanel.AddChild<Crosshair>();
		RootPanel.AddChild<Health>();
		RootPanel.AddChild<AmmoUI>();

	}
}
