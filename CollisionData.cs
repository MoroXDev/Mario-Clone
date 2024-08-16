using System.Data.Common;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;

public class CollisionData
{
  public string identifier;
  public Manifold manifold;

  public CollisionData(string identifier, Manifold manifold)
  {
    this.identifier = identifier;
    this.manifold = manifold;
  }
}