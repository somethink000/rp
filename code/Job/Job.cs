using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPSGame;
using Sandbox;

namespace FPSGame.Jobs
{
	
		public abstract partial class Job : Entity
		{
		public virtual JobType JobType => JobType.Citizen;
		public virtual int Price => 30;

		public virtual string JobName => "none";

		public virtual string Color => "white";



	}
	
}
