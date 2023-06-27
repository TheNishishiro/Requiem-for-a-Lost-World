using System;
using System.Collections.Generic;
using Objects.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public static class Utilities
	{
		public static string GetShortNumberFormatted(float number)
		{
			var suffixes = new[] {"", "K", "M", "G", "T", "P", "E", "Z", "Y", "R", "Q"};
			var suffixIndex = 0;
			var damage = number;
			while (damage > 1000)
			{
				damage /= 1000;
				suffixIndex++;
			}
			return $"{damage:0.##} {suffixes[suffixIndex]}";
		}
		
		public static string FloatToTimeString(float time)
		{
			var timeSpan = GetTimeSpan(time);
			return $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
		}
		
		public static TimeSpan GetTimeSpan(float time)
		{
			var minutes = (int)time / 60;
			var seconds = (int)time % 60;
			return new TimeSpan(0, 0, minutes, seconds);
		}
		
		public static Vector3 GetRandomInArea(Vector3 position, float size)
		{
			return new Vector3(
				Random.Range(position.x - size, position.x + size),
				Random.Range(position.y - size, position.y + size),
				Random.Range(position.z - size, position.z + size));
		}

		public static Vector3 GetRandomInAreaFreezeParameter(Vector3 position, float size, bool isFreezeX = false, bool isFreezeY = false, bool isFreezeZ = false)
		{
			return new Vector3(
				isFreezeX ? position.x : Random.Range(position.x - size, position.x + size),
				isFreezeY ? position.y : Random.Range(position.y - size, position.y + size),
				isFreezeZ ? position.z : Random.Range(position.z - size, position.z + size));
		}

		public static Vector3 GetPointOnColliderSurface(Vector3 point, Transform transform, float heightOffset = 0)
		{
			var isFound = GetPointOnColliderSurface(point, 1000f, transform, out var pointSurface);
			pointSurface.y += heightOffset;
			return isFound ? pointSurface : point;
		}
		
		public static bool GetPointOnColliderSurface(Vector3 point, float surfaceHigh, Transform transform, out Vector3 pointSurface)
		{
			var pointOnSurface = Vector3.zero;
			var pointFound = false;
			if (Physics.Raycast(point - surfaceHigh * transform.up, transform.up, out var hit, Mathf.Infinity, LayerMask.GetMask("FloorLayer")))
			{
				pointOnSurface = hit.point;
				pointFound = true;
			}
			else
			{
				if (Physics.Raycast(point + surfaceHigh * transform.up, -transform.up, out hit, Mathf.Infinity, LayerMask.GetMask("FloorLayer")))
				{
					pointOnSurface = hit.point;
					pointFound = true;
				}
			}
 
			pointSurface = pointOnSurface;
			return pointFound;
		}

		public static Damageable FindClosestDamageable(Vector3 position, IEnumerable<Damageable> enemies, out float distanceToClosest)
		{
			distanceToClosest = Mathf.Infinity;
			Damageable closest = null;

			foreach (var enemy in enemies)
			{
				var distance = Vector3.Distance(position, enemy.transform.position);
				if (distance < distanceToClosest)
				{
					distanceToClosest = distance;
					closest = enemy;
				}
			}

			return closest;
		}
		
		public static IEnumerable<Damageable> GetEnemiesInArea(Vector3 position, float radius, IEnumerable<Damageable> enemies)
		{
			var enemiesInArea = new List<Damageable>();
			foreach (var enemy in enemies)
			{
				var distance = Vector3.Distance(position, enemy.transform.position);
				if (distance < radius)
				{
					enemiesInArea.Add(enemy);
				}
			}

			return enemiesInArea;
		}

		public static Damageable FindClosestUniqueDamageable(Vector3 position, IEnumerable<Damageable> enemies, List<Damageable> foundDamagables, out float distanceToClosest)
		{
			distanceToClosest = Mathf.Infinity;
			Damageable closest = null;

			foreach (var enemy in enemies)
			{
				var distance = Vector3.Distance(position, enemy.transform.position);
				if (distance < distanceToClosest && !foundDamagables.Contains(enemy))
				{
					distanceToClosest = distance;
					closest = enemy;
				}
			}

			return closest;
		}
		
		

		public static Vector3 GenerateRandomPositionOnEdge(Vector2 spawnArea)
		{
			var position = new Vector3();

			var f = Random.value > 0.5f ? -1f : 1f;
			if (Random.value > 0.5f)
			{
				position.x = Random.Range(-spawnArea.x, spawnArea.x);
				position.z = spawnArea.y * f;
			}
			else
			{
				position.z = Random.Range(-spawnArea.y, spawnArea.y);
				position.x = spawnArea.x * f;
			}

			return position;
		}
		
	}
}