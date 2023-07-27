using Sandbox;
using System;
using System.Collections.Generic;

namespace MyGame
{
	public partial class Player
    {
		[Net] public Job PlayerJob { get; set; }


		public void Setjob( Job job )
		{
			if (PlayerJob != job )
			{
				PlayerJob = job;
			}
		}

		
	}		
}
