using System;
using UnityEngine;
using C2D.Event;

public abstract class GameComponent : EventComponent
{
	private Owned owned;
	protected Owned Owner
	{
		get
		{
			if (owned == null)
				owned = GetComponent<Owned>();
			return owned;
		}
	}
	public abstract void Setup(GameObjectData Config);

}