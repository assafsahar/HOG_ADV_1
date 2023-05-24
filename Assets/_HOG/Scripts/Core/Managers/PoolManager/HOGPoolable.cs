using HOG.Core;

public class HOGPoolable : HOGMonoBehaviour
{
    public PoolNames PoolName;
    
    virtual public void OnTakenFromPool()
    {
        this.gameObject.SetActive(true);
    }
    virtual public void OnReturnedToPool()
    {
        this.gameObject.SetActive(false);
    }

    virtual public void PreDestroy()
    {

    }
}
