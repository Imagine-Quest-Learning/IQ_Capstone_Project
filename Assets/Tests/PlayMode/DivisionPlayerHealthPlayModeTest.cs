using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/* Written by Olivia Jia
    -> Testing player health monobehavior script
*/
public class DivisionPlayerHealthPlayModeTest
{
    private GameObject playerObj;
    private PlayerHealth playerHealth;

    //init + create
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        playerObj = new GameObject();
        playerHealth = playerObj.AddComponent<PlayerHealth>();

        playerHealth.hearts = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            playerHealth.hearts[i] = new GameObject("Heart" + i);
        }

        playerHealth.life = 3;
        yield return new WaitForEndOfFrame(); // let Update() run
    }

    [UnityTest]
    public IEnumerator Hearts_Initialised()
    {
        // all hearts exist?
        Assert.IsNotNull(playerHealth.hearts[0]);
        Assert.IsNotNull(playerHealth.hearts[1]);
        Assert.IsNotNull(playerHealth.hearts[2]);

        yield return null;
    }

    [UnityTest]
    public IEnumerator Heart2_Destroyed_When_Life_Decreases_To_2()
    {
        playerHealth.TakeDamage(1); // life = 2
        yield return new WaitForEndOfFrame(); // let Update() run
        yield return null;

        Assert.IsTrue(playerHealth.hearts[2] == null ||playerHealth.hearts[2].Equals(null),"Heart 2 isn't destroyed after life=2");
    }

    [UnityTest]
    public IEnumerator Heart1_Destroyed_When_Life_Decreases_To_1()
    {
        playerHealth.TakeDamage(1); // life = 2
        yield return new WaitForEndOfFrame(); // let Update() run
        playerHealth.TakeDamage(1); // life = 1
        yield return new WaitForEndOfFrame(); // let Update() run
        yield return null;

        Assert.IsTrue(playerHealth.hearts[1] == null ||playerHealth.hearts[1].Equals(null),"Heart 1 isn't destroyed after life=1");
    }

    [UnityTest]
    public IEnumerator Heart0_Destroyed_When_Life_Decreases_To_0()
    {
        playerHealth.TakeDamage(1); // life = 2
        yield return new WaitForEndOfFrame(); // let Update() run
        playerHealth.TakeDamage(1); // life = 1
        yield return new WaitForEndOfFrame(); // let Update() run
        playerHealth.TakeDamage(1); // life = 0
        yield return new WaitForEndOfFrame(); // let Update() run
        yield return null;

        Assert.IsTrue(playerHealth.hearts[0] == null ||playerHealth.hearts[0].Equals(null),"Heart 0 isn't destroyed after life=0");
        
    }

    //clean-up, not intefered with other test
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(playerObj);
        yield return null;
    }
}
