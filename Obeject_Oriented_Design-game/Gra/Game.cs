using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gra
{
    class Game
    {
        private Map Map;

        private Player Player;

        public Map map { get => Map; }

        public Player player { get => Player; }

        private int turnCount = 0;

        private InputHandler inputHandlerChain;


        public void Run()
        {
            // Build the dungeon
            var director = new MapDirector();
            var dungeonBuilder = new DungeonBuilder();
            director.ConstructComplexDungeon(dungeonBuilder, 20, 40);
            Map = dungeonBuilder.GetMap();

            // Build instructions based on the dungeon elements
            var instructionBuilder = new InstructionBuilder();
            director.ConstructComplexDungeon(instructionBuilder, 20, 40);
            List<string> instructions = instructionBuilder.GetInstructions();
            Renderer.Instance.SetInstructions(instructions);

            BuildInputHandlerChain();

            Player = new Player(0, 0);
            turnCount = 0;

            while (true)
            {
                Renderer.Instance.Render(map, player);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                inputHandlerChain.Handle(keyInfo, this);
                player.countTurn();
            }
        }

        private void BuildInputHandlerChain()
        {
            var moveHandler = new MoveHandler();
            var combatHandler = new CombatHandler();
            var pickupHandler = new PickupHandler();
            var inventorySelectHandler = new InventorySelectHandler();
            var dropAllHandler = new DropAllHandler();
            var exitHandler = new ExitHandler();
            var invalidHandler = new InvalidInputHandler();

            moveHandler.SetNext(combatHandler)
                       .SetNext(pickupHandler)
                       .SetNext(inventorySelectHandler)
                       .SetNext(dropAllHandler)
                       .SetNext(exitHandler)
                       .SetNext(invalidHandler);

            inputHandlerChain = moveHandler;
        }


    }
}
