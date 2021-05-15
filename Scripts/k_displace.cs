
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
			newLocationV3i = displaceEntity(100, 0, 100);
			chatOutput(string.Format("Current Player Coordinates: {0},{1}  -> New Coordinates {2}, {3}", entityPlayer.GetBlockPosition().x, entityPlayer.GetBlockPosition().z, newLocationV3i.x, newLocationV3i.z));
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(buildConsoleCommand(newLocationV3i), null);
			}
			else
			{
				//SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(GameManager.Instance.World.GetPrimaryPlayerId(), "your command"), false);
			}

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
	private string buildConsoleCommand(Vector3i _location)
    {
		string outputStr = string.Format("teleport {0} {1} ", _location.x,  _location.z);
		return outputStr;
    }

	private void chatOutput(string msg)
	{
		GameManager.Instance.ChatMessageServer(_cInfo: null, _chatType: EChatType.Global, _senderEntityId: entityPlayer.entityId, _msg: msg, _mainName: entityPlayer.EntityName, _localizeMain: false, _recipientEntityIds: null);
	}

        
 }
   

