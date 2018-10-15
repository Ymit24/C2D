using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

public class Owned : EventComponent {
	private int team;
	public int Team
	{
		get
		{
			return team;
		}

		private set
		{
			team = value;
		}
	}

	public void SetTeam(int Team)
	{
		this.Team = Team;
	}
}
