using UnityEngine;
using toio;
using toio.Navigation;
using toio.MathUtils;

public class Sample_Circling_OnWebGL : MonoBehaviour
{
    CubeManager cm;
    // public Navigator.Mode naviMode = Navigator.Mode.BOIDS_AVOID;

    int Movement = 0;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        cm = new CubeManager(ConnectType.Real);
        // cm = new CubeManager();
    }

    public async void Connect()
    {
        await cm.SingleConnect();

        foreach (var navi in cm.navigators)
        {
            navi.usePred = true;
            // navi.mode = naviMode;
            navi.avoid.useSafety = false;
            navi.cube.ConfigMotorRead(true);
            // navi.AddBorder(10, 100, 400, 140, 360);
        }
    }

    public void M2RandomSingle() { 
        Movement = 2;
    }

    public void M4RandomSwarm() { 
        Movement = 4;
    }
    
    public void M5Dispersion() { 
        Movement = 5;
    }

    public void M6Circling() { 
        Movement = 6;
    }

    public void CleanCubes() {
        Movement = 0;
    }

    int M2CubesPhase = 0;
    int M5Cube1Phase, M5Cube2Phase, M5Cube3Phase, M5Cube4Phase = 0;
    

    int x1 = 130;
    int y1 = 170;
    
    int x2 = 370;
    int y2 = 170;

    int x3 = 130;
    int y3 = 330;

    int x4 = 370;
    int y4 = 330;

    void Update()
    {
        if (cm.synced){
            if (Movement == 0) {
                // Place all cubes in the corner
                cm.navigators[0].Navi2Target(130, 170, maxSpd:80).Exec();
                cm.navigators[1].Navi2Target(370, 170, maxSpd:80).Exec();
                cm.navigators[2].Navi2Target(130, 330, maxSpd:80).Exec();
                cm.navigators[3].Navi2Target(370, 330, maxSpd:80).Exec();

                Debug.Log("pos1: " + cm.navigators[0].handle.pos);
                Debug.Log("pos2: " + cm.navigators[1].handle.pos);
                Debug.Log("pos3: " + cm.navigators[2].handle.pos);
                Debug.Log("pos4: " + cm.navigators[3].handle.pos);

            } 
            
            // single Toio robot
            else if (Movement == 2) {
                if (M2CubesPhase == 0) {
                    var M2Cube1Position = cm.navigators[0].Navi2Target(130, 170, maxSpd:80).Exec();
                    var M2Cube2Position = cm.navigators[1].Navi2Target(370, 230, maxSpd:80).Exec();
                    var M2Cube3Position = cm.navigators[2].Navi2Target(370, 280, maxSpd:80).Exec();
                    var M2Cube4Position = cm.navigators[3].Navi2Target(370, 330, maxSpd:80).Exec();

                    if (M2Cube1Position.reached && M2Cube2Position.reached && M2Cube3Position.reached && M2Cube4Position.reached) {
                        M2CubesPhase = 1;
                    }
                }
                else if (M2CubesPhase == 1) {
                    var navi = cm.navigators[0];
                    var M2CubePosition = navi.Navi2Target(x1, y1, maxSpd:50).Exec();
                    if (M2CubePosition.reached) {
                        x1 = Random.Range(130, 370);
                        y1 = Random.Range(170, 330);
                    }
                    else if (navi.cube.leftSpeed == 0 && navi.cube.rightSpeed == 0) {
                        x1 = Random.Range(130, 370);
                        y1 = Random.Range(170, 330);
                    }
                }
            } 
            
            // swarm toio robots
            else if (Movement == 4) {
                M2CubesPhase = 0; // restore M2CubesPhase

                var navi1 = cm.navigators[0];
                var navi2 = cm.navigators[1];
                var navi3 = cm.navigators[2];
                var navi4 = cm.navigators[3];

                // Cube 1
                var M4Cube1Position = navi1.Navi2Target(x1, y1, maxSpd:50).Exec();
                if (M4Cube1Position.reached) {
                    x1 = Random.Range(130, 370);
                    y1 = Random.Range(170, 330);
                    
                }
                else if (navi1.cube.leftSpeed == 0 && navi1.cube.rightSpeed == 0) {
                    x1 = Random.Range(130, 370);
                    y1 = Random.Range(170, 330);
                }

                // Cube 2
                var M4Cube2Position = navi2.Navi2Target(x2, y2, maxSpd:50).Exec();
                if (M4Cube2Position.reached) {
                    x2 = Random.Range(130, 370);
                    y2 = Random.Range(170, 330);
                }
                else if (navi2.cube.leftSpeed == 0 && navi2.cube.rightSpeed == 0) {
                    x2 = Random.Range(130, 370);
                    y2 = Random.Range(170, 330);
                }

                // Cube 3
                var M4Cube3Position = navi3.Navi2Target(x3, y3, maxSpd:50).Exec();
                if (M4Cube3Position.reached) {
                    x3 = Random.Range(130, 370);
                    y3 = Random.Range(170, 330);
                }
                else if (navi3.cube.leftSpeed == 0 && navi3.cube.rightSpeed == 0) {
                    x3 = Random.Range(130, 370);
                    y3 = Random.Range(170, 330);
                }

                // Cube 4
                var M4Cube4Position = navi4.Navi2Target(x4, y4, maxSpd:50).Exec();
                if (M4Cube4Position.reached) {
                    x4 = Random.Range(130, 370);
                    y4 = Random.Range(170, 330);
                }
            } 
            
            // swarm dispersion
            else if (Movement == 5) {
                M2CubesPhase = 0; // restore M2CubesPhase

                if (M5Cube1Phase == 0 && M5Cube2Phase == 0){
                    Movement M5Cube1Position = cm.navigators[0].handle.Move2Target(250, 160).Exec();
                    Movement M5Cube2Position = cm.navigators[1].handle.Move2Target(160, 250).Exec();
                    Movement M5Cube3Position = cm.navigators[2].handle.Move2Target(250, 340).Exec();
                    Movement M5Cube4Position = cm.navigators[3].handle.Move2Target(340, 250).Exec();

                    if (
                        M5Cube1Position.reached && 
                        M5Cube2Position.reached &&
                        M5Cube3Position.reached &&
                        M5Cube4Position.reached
                    ) {
                        M5Cube1Phase = 1;
                        M5Cube2Phase = 1;
                        M5Cube3Phase = 1;
                        M5Cube4Phase = 1;
                    }
                }
                else if (M5Cube1Phase == 1 && M5Cube2Phase == 1 && M5Cube3Phase == 1 && M5Cube4Phase == 1) {
                    Movement M5Cube1Direction = cm.navigators[0].handle.Rotate2Deg(-90).Exec();
                    Movement M5Cube2Direction = cm.navigators[1].handle.Rotate2Deg(180).Exec();
                    Movement M5Cube3Direction = cm.navigators[2].handle.Rotate2Deg(90).Exec();
                    Movement M5Cube4Direction = cm.navigators[3].handle.Rotate2Deg(0).Exec();

                    if (
                        M5Cube1Direction.reached && 
                        M5Cube2Direction.reached &&
                        M5Cube3Direction.reached &&
                        M5Cube4Direction.reached
                    ) {
                        M5Cube1Phase = 2;
                        M5Cube2Phase = 2;
                        M5Cube3Phase = 2;
                        M5Cube4Phase = 2;
                    }
                }
                else if (M5Cube1Phase == 2 && M5Cube2Phase == 2 && M5Cube3Phase == 2 && M5Cube4Phase == 2) {
                    cm.navigators[0].cube.Move(-40, -40, 90);
                    cm.navigators[1].cube.Move(-40, -40, 90);
                    cm.navigators[2].cube.Move(-40, -40, 90);
                    cm.navigators[3].cube.Move(-40, -40, 90);

                    if (cm.navigators[0].handle.y >= 210) {
                        M5Cube1Phase = 0;
                        M5Cube2Phase = 0;
                        M5Cube3Phase = 0;
                        M5Cube4Phase = 0;
                    }
                }
            } 
            
            else if (Movement == 6) {
                M2CubesPhase = 0; // restore M2CubesPhase

                for (int i=0; i<cm.navigators.Count; i++)
                {
                    var angleOffset = Mathf.PI * 2 / cm.navigators.Count * i;
                    var tar = Vector.fromRadMag(Time.time/1 + angleOffset, 80) + new Vector(250, 250);
                    var navi = cm.navigators[i];
                    var mv = navi.Navi2Target(tar, maxSpd:60).Exec();
                }
            }
        }
    }
}
