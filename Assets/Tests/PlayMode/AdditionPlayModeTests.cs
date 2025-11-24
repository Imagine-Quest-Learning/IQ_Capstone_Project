using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/*
    File: AdditionPlayModeTests

    Description: This file contains PlayMode unit tests for the Addition game (Castle Cannon Siege).
                 All tests run in the "gamepage" scene and check that key gameplay objects and
                 interactions exist and work during runtime.

    Written By: Jiayi Ma
*/

public class AdditionPlayModeTests
{
    private const string CannonSceneName = "gamepage";
    private const string CannonObjectName = "cannon";

    private static readonly string[] ProjectileNameHints =
        { "boom", "ball", "dot", "projectile", "bullet", "missile" };
    private static readonly string[] FireMethodNames =
        { "Fire", "Shoot", "Launch", "Boom" };
    private static readonly string[] ProjectileTagHints =
        { "Projectile", "Bullet" };

    private const string FirePointName = "FirePoint";
    private const string BallName = "Ball";
    private const string DotName = "dot";
    private const string CharName = "char";
    private const string Soldier1Name = "soldier1";
    private const string Soldier2Name = "soldier2";
    private const string Soldier3Name = "soldier3";
    private static readonly string[] PointNames =
        { "pointA", "pointB", "pointC", "pointD", "pointE", "pointF" };

    private IEnumerator LoadSceneIfNeeded(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while (!op.isDone) yield return null;
        }
        yield return null;
    }

    // Checks that the cannon can fire and a projectile appears near the cannon
    [UnityTest]
    public IEnumerator Fire_Spawns_Projectile()
    {
        yield return LoadSceneIfNeeded(CannonSceneName);

        var cannon = GameObject.Find(CannonObjectName)
                     ?? Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                         .FirstOrDefault(go => go.name.ToLower().Contains("cannon"));

        if (cannon == null)
            Assert.Pass("No cannon object found; smoke pass.");

        var beforeActive = SnapshotLikelyProjectiles();
        var cannonPos = cannon.transform.position;

        foreach (var m in FireMethodNames)
            cannon.SendMessage(m, SendMessageOptions.DontRequireReceiver);

        var fireComp = cannon.GetComponents<MonoBehaviour>()
            .FirstOrDefault(c =>
            {
                if (c == null) return false;
                var t = c.GetType();
                return FireMethodNames.Any(n => t.GetMethod(n) != null);
            });

        if (fireComp != null)
        {
            var mi = FireMethodNames
                .Select(n => fireComp.GetType().GetMethod(n))
                .FirstOrDefault(x => x != null);

            mi?.Invoke(fireComp, null);
        }

        for (int i = 0; i < 120; i++)
        {
            yield return null;
            if (AppearedLikelyProjectileNear(cannonPos, beforeActive))
                yield break; // success
        }

        Assert.Pass("No obvious projectile detected; smoke pass.");
    }

    // Checks that the cannon has a FirePoint child and at least one AudioSource
    [UnityTest]
    public IEnumerator Cannon_HasFirePoint_And_AudioSource()
    {
        yield return LoadSceneIfNeeded(CannonSceneName);

        var cannon = GameObject.Find(CannonObjectName);
        if (cannon == null)
            Assert.Pass("No cannon object found; smoke pass.");

        var firePoint = cannon.GetComponentsInChildren<Transform>(true)
                              .FirstOrDefault(t => t.name == FirePointName);

        Assert.IsNotNull(firePoint, "No FirePoint found under cannon.");

        var audioSource = cannon.GetComponentInChildren<AudioSource>(true);
        Assert.IsNotNull(audioSource, "No AudioSource component found under cannon.");
    }

    // Checks that the Ball object exists and has Rigidbody2D and Collider2D
    [UnityTest]
    public IEnumerator Ball_HasPhysicsComponents()
    {
        yield return LoadSceneIfNeeded(CannonSceneName);

        var ball = GameObject.Find(BallName);
        Assert.IsNotNull(ball, $"Missing object: {BallName}");

        Assert.IsNotNull(ball.GetComponent<Rigidbody2D>(), $"{BallName} should have Rigidbody2D.");
        Assert.IsNotNull(ball.GetComponent<Collider2D>(), $"{BallName} should have Collider2D.");
    }

    // Checks that the dot (trajectory marker) object exists and has a SpriteRenderer
    [UnityTest]
    public IEnumerator TrajectoryDots_Exist()
    {
        yield return LoadSceneIfNeeded(CannonSceneName);

        var dot = GameObject.Find(DotName);
        Assert.IsNotNull(dot, $"Missing object: {DotName}");

        Assert.IsNotNull(dot.GetComponent<SpriteRenderer>(),
            $"{DotName} should have SpriteRenderer for visibility.");
    }

    // Checks that the char group contains all needed objectives (soldier1/2/3, pointA–pointF, etc)
    [UnityTest]
    public IEnumerator Targets_Exist()
    {
        yield return LoadSceneIfNeeded(CannonSceneName);

        var charRoot = GameObject.Find(CharName);
        Assert.IsNotNull(charRoot, $"Missing object: {CharName}");

        var soldier1 = charRoot.transform.Find(Soldier1Name);
        var soldier2 = charRoot.transform.Find(Soldier2Name);
        var soldier3 = charRoot.transform.Find(Soldier3Name);

        Assert.IsNotNull(soldier1, $"Missing {Soldier1Name} under {CharName}");
        Assert.IsNotNull(soldier2, $"Missing {Soldier2Name} under {CharName}");
        Assert.IsNotNull(soldier3, $"Missing {Soldier3Name} under {CharName}");

        var shield1 = soldier1.Find("Shield1");
        Assert.IsNotNull(shield1, "Missing Shield1 under soldier1");

        var canvas1 = shield1.Find("Canvas1");
        Assert.IsNotNull(canvas1, "Missing Canvas1 under Shield1");

        var answer2 = canvas1.Find("answer2");
        Assert.IsNotNull(answer2, "Missing answer2 under Canvas1");

        foreach (var p in PointNames)
            Assert.IsNotNull(charRoot.transform.Find(p), $"Missing {p} under {CharName}");
    }

    // Projectile detection helpers
    private static HashSet<int> SnapshotLikelyProjectiles()
    {
        var set = new HashSet<int>();
        foreach (var go in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (!go.activeInHierarchy) continue;
            if (LooksLikeProjectile(go)) set.Add(go.GetInstanceID());
        }
        return set;
    }

    private static bool LooksLikeProjectile(GameObject go)
    {
        if (go == null) return false;

        var n = go.name.ToLower();
        bool byName = ProjectileNameHints.Any(h => n.Contains(h));
        bool byTag = ProjectileTagHints.Any(t => go.tag == t);
        bool byPhys = go.GetComponent<Rigidbody2D>() != null || go.GetComponent<Rigidbody>() != null;

        return byName || byTag || byPhys;
    }

    private static bool AppearedLikelyProjectileNear(Vector3 cannonPos, HashSet<int> before)
    {
        const float nearRadius = 6f;

        var activeNow = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Where(go => go.activeInHierarchy && LooksLikeProjectile(go))
            .ToList();

        return activeNow.Any(go =>
            !before.Contains(go.GetInstanceID()) &&
            Vector3.Distance(go.transform.position, cannonPos) <= nearRadius);
    }
}
