using Godot;

namespace Game{
    public static class Util{

        public const float coyoteTime = 0.075f;

        public static Node2D GetEntGrp(Node treeNode){
            return treeNode.GetNode<Node2D>("/root/GameController/GameMap/ents");
        }

        public static GameController GetGameController(Node treeNode){
            return treeNode.GetNode<GameController>("/root/GameController");
        }
    }
}