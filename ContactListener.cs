using System.Numerics;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using QuickType;
using static MainCode;

public class ContactListener : IContactListener
{
  Player kosmo;
  List<Entity> entities;

  public ContactListener(ref Player player, ref List<Entity> entities)
  {
    kosmo = player;
    this.entities = entities;
  }

  public void BeginContact(Contact contact)
  {
    Body bodyA = contact.FixtureA.Body;
    Body bodyB = contact.FixtureB.Body;
    Entity entityA = FindEntity((bodyA.UserData as BodyData).Iid, entities.ToArray());
    Entity entityB = FindEntity((bodyB.UserData as BodyData).Iid, entities.ToArray());

    entityA.CollisionStart((BodyData)bodyA.UserData, (BodyData)bodyB.UserData, -contact.Manifold.LocalNormal);
    entityB.CollisionStart((BodyData)bodyB.UserData, (BodyData)bodyA.UserData, contact.Manifold.LocalNormal);
  }

  public void EndContact(Contact contact)
  {
    Body bodyA = contact.FixtureA.Body;
    Body bodyB = contact.FixtureB.Body;
    Entity entityA = FindEntity((bodyA.UserData as BodyData).Iid, entities.ToArray());
    Entity entityB = FindEntity((bodyB.UserData as BodyData).Iid, entities.ToArray());

    entityA.CollisionEnd((BodyData)bodyA.UserData, (BodyData)bodyB.UserData, -contact.Manifold.LocalNormal);
    entityB.CollisionEnd((BodyData)bodyB.UserData, (BodyData)bodyA.UserData, contact.Manifold.LocalNormal);
  }

  public void PostSolve(Contact contact, in ContactImpulse impulse)
  {

  }

  public void PreSolve(Contact contact, in Manifold oldManifold)
  {

  }

  /// <summary>
  /// Returns true if both bodies have same identifiers as arguments
  /// </summary>
  /// <param name="bodyA"></param>
  /// <param name="BodyB"></param>
  /// <param name="identifierA"></param>
  /// <param name="identifierB"></param>
  /// <returns></returns>
  bool CheckIdentifiers(Body bodyA, Body BodyB, string identifierA, string identifierB)
  {

    if ((bodyA.UserData as BodyData).Identifier == identifierA && (BodyB.UserData as BodyData).Identifier == identifierB)
    {
      return true;
    }

    if ((bodyA.UserData as BodyData).Identifier == identifierB && (BodyB.UserData as BodyData).Identifier == identifierA)
    {
      return true;
    }

    return false;
  }

  Body GetNotPlayerBody(Body bodyA, Body BodyB)
  {
    if ((bodyA.UserData as BodyData).Identifier == "Player")
    {
      return BodyB;
    }
    else return bodyA;
  }
}