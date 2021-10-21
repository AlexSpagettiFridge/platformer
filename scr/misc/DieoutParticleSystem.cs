using Godot;
using System;

namespace Game{

public class DieoutParticleSysteme : Particles2D
{
    public override void _Ready()
    {
        GetNode<Timer>("Timer").Connect("timeout",this,nameof(OnTimout));
    }

    private void OnTimout(){
        QueueFree();
    }
}
}