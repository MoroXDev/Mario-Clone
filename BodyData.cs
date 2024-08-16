using System.Security.Principal;

public class BodyData
{
  public string Identifier;
  public string Iid;

  public BodyData(string Iid, string Identifier)
  {
    this.Iid = Iid;
    this.Identifier = Identifier;
  }
}