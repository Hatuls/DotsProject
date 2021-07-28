using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;



// inherit from componentSystem and implement the OnUpdate system function
public class MovementSystem : ComponentSystem
{
    // the Update method
    protected override void OnUpdate()
    {
        Entities.WithAll<MoveForward>().ForEach((ref Translation trans,ref Rotation rot, ref MoveForward moveForward)
            =>
        {
            if (moveForward.toMove)
                         trans.Value += moveForward.Speed * Time.DeltaTime * math.forward(rot.Value);

        }
        );
    }
}
