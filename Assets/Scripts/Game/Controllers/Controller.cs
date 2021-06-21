#nullable enable

using UnityEngine;

using Zongband.Game.Commands;
using Zongband.Game.Entities;

namespace Zongband.Game.Controllers
{
    public abstract class Controller : MonoBehaviour
    {
        public abstract Command? ProduceCommand(Agent agent, Command.Context ctx);
    }
}