public interface ILivingEntity
{
    public float HP { get; set; }

    public void OnDeath(int damage);
    public void OnDamaged();
}
