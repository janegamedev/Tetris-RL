using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class TetrisAgent : Agent
{
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void Heuristic(float[] actionsOut)
    {
        base.Heuristic(actionsOut);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        base.OnActionReceived(vectorAction);
    }
}
