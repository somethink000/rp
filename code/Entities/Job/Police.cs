using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
	 class Police : Job
	{
		public override JobType JobType => JobType.Citizen;
		public override int Price => 60;

		public override string JobName => "Police";

		public override string Color => "green";
	}
}
