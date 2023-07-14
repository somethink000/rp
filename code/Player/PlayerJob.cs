using FPSGame.Jobs;
using Sandbox;
using System;
using System.Collections.Generic;

namespace FPSGame
{
	public partial class FPSPlayer
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
