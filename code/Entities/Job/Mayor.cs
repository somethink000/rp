﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
	partial class Mayor : Job
	{
		public override JobType JobType => JobType.Citizen;
		public override int Price => 100;

		public override string JobName => "Mayor";

		public override string Color => "green";
	}
}
