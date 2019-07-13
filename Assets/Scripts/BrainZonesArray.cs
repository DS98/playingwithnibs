using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Application
{
  public class BrainZonesArray {
    public BrainZone[] brainZones;

    public  int countActiveZones;

    public BrainZonesArray(List<BrainZone> brainZonesList) {
      brainZones = new BrainZone[6];
      Stimulator stimulator = new Stimulator();
      
      brainZones[0] = 
        new BrainZone(BrainZoneNames.DLPFC, Position.UPPER, stimulator);
      brainZones[1] = 
        new BrainZone(BrainZoneNames.M1, Position.UPPER, stimulator);
      brainZones[2] = 
        new BrainZone(BrainZoneNames.SO, Position.UPPER, stimulator);
      brainZones[3] = 
        new BrainZone(BrainZoneNames.O, Position.UPPER, stimulator);
      brainZones[4] = 
        new BrainZone(BrainZoneNames.CP5, Position.LEFT, stimulator);
      brainZones[5] = 
        new BrainZone(BrainZoneNames.CP6, Position.RIGHT, stimulator);

      countActiveZones = 0;

      Debug.Log("inside constructor");
      brainZonesList.ForEach(bz => {
        if (stimulator.electrodeType != ElectrodeType.NO) {
          Debug.Log(bz.brainZoneName);
          Debug.Log(bz.position);
          Debug.Log(bz.stimulator);
          brainZones[(int)bz.brainZoneName].position = bz.position;
          activateZone(bz.brainZoneName, bz.stimulator);
        }
        
      });
    }

    // tested
    public void activateZone(BrainZoneNames brainZoneName, 
      Stimulator stimulator) {
        if (stimulator.electrodeType != ElectrodeType.NO) {
          brainZones[(int)brainZoneName].stimulator.electrodeType 
            = stimulator.electrodeType;

          countActiveZones++;
        }
    }

    public void activateZone(int brainZoneName,
      Stimulator stimulator) {
      if (stimulator.electrodeType != ElectrodeType.NO)
        activateZone((BrainZoneNames)brainZoneName, stimulator);
    }

    // tested
    public void deactivateZone(BrainZoneNames brainZoneName) {
      brainZones[(int)brainZoneName].stimulator.electrodeType 
        = ElectrodeType.NO;

      countActiveZones--;
    }

    public override string ToString() {
      string result = "";

      for (int i = 0; i < 6; i++) {
        result += brainZones[i].brainZoneName + ", " + brainZones[i].position +
          ", " + brainZones[i].stimulator + "\n";
      }

      return result;
    }
  }
}