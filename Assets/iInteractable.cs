using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iInteractable {

	void OnHoverMe(Player player);

	void OnStopHoverMe(Player player);
	void OnInteractWithMe(Player player);	

	//void SetParentRoomDependencies();
}
