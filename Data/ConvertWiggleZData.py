import numpy as np
from astropy.coordinates import Angle
from astropy import units as u
from astropy import coordinates as coord
from astropy.table import Table
from astropy.io import ascii

# Import required data, reshuffle, and get random sample
print("\nImporting data...")
data = np.genfromtxt("WiggleZData.txt", dtype="str", delimiter=" ")

# Slice data array to get values of interest
print("\nSlicing arrays...")
RAStrings = data[:,1]
DecStrings = data[:,2]
RedshiftStrings = data[:,3]
RedBandStrings = data[:,9]
GreenBandStrings = data[:,7]
BlueBandStrings = data[:,6]

# Address blank redshift values by setting them at very high redshift (out of sight)
print("\nRemoving blank redshifts...")
for i in range(len(RedshiftStrings)):
    if RedshiftStrings[i] == '-':
        RedshiftStrings[i] = 1000

# Convert these string values to values that astropy can manage
print("\nConverting arrays...")
RAValues = Angle(RAStrings, u.degree)
RAValues = np.radians(RAValues)
DecValues = Angle(DecStrings, u.degree)
DecValues = np.radians(DecValues)
RedshiftValues = RedshiftStrings.astype(np.float)
RedBandValues = RedBandStrings.astype(np.float)
GreenBandValues = GreenBandStrings.astype(np.float)
BlueBandValues = BlueBandStrings.astype(np.float)


# Assign RGB values for each galaxy
print("\nGetting colour values...")
cutoff = 10000
gamma = 2.2

RedIntensities = 10 ** RedBandValues
Red = RedIntensities ** gamma
RedValues = np.interp(Red, np.array([np.sort(Red)[cutoff], np.sort(Red)[len(Red) - cutoff]]), np.array([0.0, 0.75]))

GreenIntensities = 10 ** GreenBandValues
Green = GreenIntensities ** gamma
GreenValues = np.interp(Green, np.array([np.sort(Green)[cutoff], np.sort(Green)[len(Green) - cutoff]]), np.array([0.0, 0.2]))

BlueIntensities = 10 ** BlueBandValues
Blue  = BlueIntensities ** gamma
BlueValues = np.interp(Blue, np.array([np.sort(Blue)[cutoff], np.sort(Blue)[len(Blue)-cutoff]]), np.array([0.4, 1.0]))

print("\nGetting cartesian values...")
multiplier = 100 # The factor to spread it out in Unity visualisation space, equivalent to redshiftScale in CreateGrid.cs
XArray = RedshiftValues * np.cos(DecValues) * np.cos(RAValues) * multiplier
YArray = RedshiftValues * np.cos(DecValues) * np.sin(RAValues) * multiplier
ZArray = RedshiftValues * np.sin(DecValues) * multiplier

# Assign values to table
print("\nWriting data to table...")
dataTable = Table({'x' : XArray, 'y' : YArray, 'z' : ZArray, 'R' : RedValues, 'G' : GreenValues, 'B' : BlueValues}, names=("x", "y", "z", "R", "G", "B"))

# Write to file
print("\nWriting to file...")
ascii.write(dataTable, "../Assets/Resources/WiggleZProcessed.txt", delimiter=",", format="no_header")

print("\nDone!\n")