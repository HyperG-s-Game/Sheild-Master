using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using GamerWolf.Utils.HealthSystem;

namespace InkShield {
    public enum EnemyType{
        Normal,Armourd,Super
    }
    
    public class EnemyController : HealthEntity {
        [SerializeField] protected EnemyType enemyType;

        [Header("Enemy Shooting")]
        [SerializeField] private GameObject wepon;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private float maxFireTime = 4f;

        [Header("External References")]
        [SerializeField] private EnemyAnimationHandler animationHandler;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Rotator[] rotators;
        
        private ObjectPoolingManager objectPoolingManager;
        private PlayerController player;
        private string timerName = "Fire Timer";
        
        protected override void Awake(){
            base.Awake();
        }
        
        protected override void Start(){
            
            base.OnHit += (object sender ,EventArgs e) => {
                healthBar.UpdateHealthBar(base.GetHealthNormalized(),transform);
            };
            healthBar.HideHealthBar();
            base.Start();
            player = PlayerController.player;
            objectPoolingManager = ObjectPoolingManager.current;
        }
        
        public void StartEnemy(){
            Debug.Log("On Game Start");
            StartCoroutine(nameof(ShootingRoutine));
            base.onDead += (object sender,EventArgs e) =>{
                animationHandler.PlayIsDeadAnimations();
                StopCoroutine(nameof(ShootingRoutine));
            };
            GameHandler.current.onGameOver += (object sender,OnGamoverEventsAargs e)=> {
                StopCoroutine(nameof(ShootingRoutine));
                wepon.SetActive(false);
            };
        }
        
        
        private IEnumerator ShootingRoutine(){
            yield return new WaitForSeconds(1f);
            while(!isDead){
                Fire();
                
                yield return new WaitForSeconds(maxFireTime);
            }
        }
        
        public void EndGame(){
            StopCoroutine(nameof(ShootingRoutine));
        }
        private void Update(){
            if(!isDead){
                RotateTowardsPlayer();
            }
        }
        

        protected virtual void Fire(){
            
            wepon.SetActive(true);
            GameObject projectile =  objectPoolingManager.SpawnFromPool(PoolObjectTag.Projectile,firePoint.position,firePoint.rotation);
            Projectile bullet = projectile.GetComponent<Projectile>();
            if(bullet != null){
                bullet.SetCameFromEnemy(this);
            }
            animationHandler.PlayShootingAnimations();
            
        }

        
        
        private void RotateTowardsPlayer(){
            for (int i = 0; i < rotators.Length; i++){
                rotators[i].Rotate(player.transform);
            }
        }
        
        public EnemyType GetEnemyType(){
            return enemyType;
        }
        
        
    }

}