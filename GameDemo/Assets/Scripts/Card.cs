public class Card 
{
    public int id;
    public int Cost;
    public string CardName;
    
    public Card(int _id, int _Cost, string _CardName)
    {
        this.id = _id;
        this.Cost = _Cost;
        this.CardName = _CardName;
    } 
}

public class AttackCard : Card
{
    public int Amount;
    
    public AttackCard(int _id, int _Cost, string _CardName, int _Amount) : base(_id, _Cost, _CardName)
    {
        this.id = _id;
        this.Cost = _Cost;
        this.CardName = _CardName;    
        this.Amount = _Amount;    
    }
}

public class DefendCard : Card
{
    public int Amount;
    
    public DefendCard(int _id, int _Cost, string _CardName, int _Amount) : base(_id, _Cost, _CardName)
    {
        this.id = _id;
        this.Cost = _Cost;
        this.CardName = _CardName;
        this.Amount = _Amount;    
    }
}

public class SkillCard : Card
{
    public string Effect;
    
    public SkillCard(int _id, int _Cost, string _CardName, string _Effect) : base(_id, _Cost, _CardName)
    {
        this.id = _id;
        this.Cost = _Cost;
        this.CardName = _CardName;    
        this.Effect = _Effect;    
    }
}