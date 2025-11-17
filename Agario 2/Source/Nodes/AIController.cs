using MyEngine.Nodes;
using MyEngine.Nodes.Controllers;
using MyEngine.Utils;
using SFML.System;

namespace Agario_2.Nodes;

public class AiController : Controller<Player>
{
    private Vector2f _currentWayPoint;

    private const float MaxDistanceToWayPoint = 500;
    private const float DistanceSquaredForNewWayPoint = 100;

    private const float ThreatRadius = 700f;
    private const float FleeSafeRadius = 900f;
    private const float HuntRadius = 800f;
    private const float SizeAdvantageToHunt = 1.15f;
    private const float SizeDisadvantageToFlee = 1.10f;
    private const float FleeBiasMultiplier = 1.0f;
    private const float HuntBiasMultiplier = 0.8f;

    private Vector2f Position
        => Controlled?.Position ?? _currentWayPoint;
    
    private AiController()
    { }
    
    protected override void SetControlled(Player newControlled)
    {
        base.SetControlled(newControlled);
        
        _currentWayPoint = newControlled.Position;
        SetNewWayPoint();
    }

    public static AiController CreateAiController(Player player)
    {
        AiController result = new();
        
        result.Controlled = player;
        result.SetNewWayPoint();

        return result;
    }

    private void SetNewWayPoint()
    {
        float x = MyRandom.GetFloatInDistance(Position.X, MaxDistanceToWayPoint);
        float y = MyRandom.GetFloatInDistance(Position.Y, MaxDistanceToWayPoint);
        
        _currentWayPoint = new(x, y);
    }
    
    public override void Update(in UpdateInfo info)
    {
        if (Controlled == null)
            return;
        
        Vector2f desired;
        if (TryComputeFleeVector(out Vector2f flee))
        {
            desired = flee * FleeBiasMultiplier;
        }
        else if (TryComputeHuntVector(out Vector2f hunt))
        {
            desired = hunt * HuntBiasMultiplier;
        }
        else
        {
            if (TooCloseToWayPoint())
                SetNewWayPoint();
            desired = (_currentWayPoint - Controlled.Position);
        }

        SetDeltaFromDesired(desired);
    }

    private bool TooCloseToWayPoint()
        => Controlled.Position.SquaredDistanceTo(_currentWayPoint) < DistanceSquaredForNewWayPoint;
    
    private void SetDeltaFromDesired(in Vector2f desired)
    {
        float len = desired.Length();
        if (len <= 0.0001f)
        {
            Controlled.WishedDelta = new(0, 0);
            return;
        }

        Vector2f delta = desired / len * Controlled.MaxSpeed;
        Controlled.WishedDelta = delta;
    }

    private bool TryComputeFleeVector(out Vector2f flee)
    {
        flee = new(0, 0);
        List<Player> players = GetRootNode().GetChildrenOfType<Player>();
        float myR = Controlled.CurrentRadius;

        bool found = false;
        foreach (Player p in players)
        {
            if (p == Controlled)
                continue;

            float theirR = p.CurrentRadius;
            if (theirR < myR * SizeDisadvantageToFlee)
                continue;

            float distSq = Controlled.Position.SquaredDistanceTo(p.Position);
            float threatRadiusSq = ThreatRadius * ThreatRadius;
            if (distSq > threatRadiusSq)
                continue;

            Vector2f away = Controlled.Position - p.Position;
            float len = float.Sqrt(distSq);
            if (len > 0.0001f)
            {
                float closeness = 1f - float.Clamp(len / FleeSafeRadius, 0f, 1f);
                float sizeFactor = float.Clamp((theirR / myR) - 1f, 0f, 1.5f);
                flee += away / len * (closeness + sizeFactor);
                found = true;
            }
        }

        return found;
    }

    private bool TryComputeHuntVector(out Vector2f hunt)
    {
        hunt = new(0, 0);
        List<Player> players = GetRootNode().GetChildrenOfType<Player>();
        float myR = Controlled.CurrentRadius;

        float bestScore = 0f;
        Vector2f bestDir = new(0, 0);

        foreach (Player p in players)
        {
            if (p == Controlled)
                continue;

            float theirR = p.CurrentRadius;
            if (myR < theirR * SizeAdvantageToHunt)
                continue;

            float distSq = Controlled.Position.SquaredDistanceTo(p.Position);
            if (distSq > HuntRadius * HuntRadius)
                continue;

            float dist = float.Sqrt(distSq);
            if (dist <= 0.0001f)
                continue;
            
            float sizeAdv = float.Clamp(myR / float.Max(1e-3f, theirR), 1f, 4f);
            float proximity = 1f - float.Clamp(dist / HuntRadius, 0f, 1f);
            float score = proximity * sizeAdv;

            if (score > bestScore)
            {
                bestScore = score;
                bestDir = (p.Position - Controlled.Position) / dist;
            }
        }

        if (bestScore > 0f)
        {
            hunt = bestDir;
            return true;
        }

        return false;
    }
}