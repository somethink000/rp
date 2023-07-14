﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPSGame.Jobs
{
	partial class Bandit : Job
	{
		public override JobType JobType => JobType.Citizen;

		public override int Price => 30;

		public override string JobName => "Бандит";

		public override string Color => "green";
	}
}