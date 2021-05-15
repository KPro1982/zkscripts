
using System;
using System.Linq;
using System.Collections.Generic;

public class k_displace : ConsoleCmdAbstract
  {
      private EntityPlayer entityPlayer;
	  private Vector3i newLocationV3i = new Vector3i(0,0,0);

	  public override string[] GetCommands() => new[] {"k_displace"};

      public override string GetDescription() => "Get location of player entity.";

      public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
    {
      
		if (!_senderInfo.IsLocalGame && _senderInfo.RemoteClientInfo == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients.");
			return;
		}

		
		if (_senderInfo.IsLocalGame)
		{
			entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			newLocationV3i = displaceEntity(100, 100, 100);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Current Player Coordinates:." + entityPlayer.GetBlockPosition());
			chatOutput("Current Player Coordinates: " + entityPlayer.GetBlockPosition() + "  New Coordinates: " + newLocationV3i.ToString());
			ConsoleCmdTeleport.Execute(convertToList(newLocationV3i),_senderInfo);
			
		}
		else
		{
			entityPlayer = GameManager.Instance.World.Players.dict[_senderInfo.RemoteClientInfo.entityId];
		}


	}
	private Vector3i  displaceEntity(int _x, int _y, int _z)
    {
		Vector3i displaceAmount = new Vector3i(_x, _y, _z);
		return entityPlayer.GetBlockPosition() + displaceAmount;
	}
	private List<String> convertToList(Vector3i _location)
    {
		List<string> outputList = new List<string>();
		outputList.Add(_location.ToString());
		return outputList;
    }

	private void chatOutput(string msg)
	{
		GameManager.Instance.ChatMessageServer(_cInfo: null, _chatType: EChatType.Global, _senderEntityId: entityPlayer.entityId, _msg: msg, _mainName: entityPlayer.EntityName, _localizeMain: false, _recipientEntityIds: null);
	}

        
 }
   

