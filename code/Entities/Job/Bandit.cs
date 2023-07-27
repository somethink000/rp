using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
	partial class Bandit : Job
	{
		public override JobType JobType => JobType.Citizen;

		public override int Price => 30;

		public override string JobName => "";

		public override string Color => "green";
	}
}
