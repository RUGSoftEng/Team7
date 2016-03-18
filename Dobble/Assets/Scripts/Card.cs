﻿using UnityEngine;
using System.Collections;
using System;

// circle packing: http://hydra.nat.uni-magdeburg.de/packing/cci/cci.html

public class Card : MonoBehaviour {

	public Symbol symbolPrefab;

	// array of contained symbols
	private Symbol[] containedSymbols; 
	
	public void Constructor() {

		//this.transform.SetParent (parent);

		string[] lines = System.IO.File.ReadAllLines(@"Assets/Resources/Circle packings/8.txt");
		float radius = float.Parse (lines [0]);
		Vector2[] coordinates = new Vector2[8];
		string[] line;
		for (int i = 1; i < 8+1; ++i) {
			line = lines[i].Split(' ');
			coordinates[i-1] = new Vector2(float.Parse(line[0]), float.Parse(line[1]));
		}
		
		Sprite[] sprites = Resources.LoadAll<Sprite>("Symbols");
		float[] scales = new float[sprites.Length];
		for (int i = 0; i < sprites.Length; ++i) {
			Vector3 size = sprites[i].bounds.size;
			scales[i] = radius*2/Mathf.Sqrt(size.x*size.x+size.y*size.y);
		}
		
		this.containedSymbols = new Symbol[8];
		for (int i = 0; i < 8; ++i) (this.containedSymbols[i] = (Symbol) Instantiate (symbolPrefab)).Constructor(this.transform, coordinates[i], sprites, scales);

	}

	// true if this card contains the symbol
	public bool ContainsSymbol(int symbol) {
		foreach (Symbol s in this.containedSymbols) if (s.getSymbol() == symbol) return true;
		return false;
	}

	public int[] GetCard() {
		int[] c = new int[8];
		int i = 0;
		foreach (Symbol s in this.containedSymbols) {
			c[i] = s.getSymbol();
			++i;
		}
		return c;
	}

	// sets card and a random rotation
	public void SetCard(int[] card) {
		int symbol = 0;
		foreach (Symbol s in this.containedSymbols) {
			s.SetSymbol(card[symbol]);
			++symbol;
		}

		// apply a random rotation to the card
		this.transform.eulerAngles = Vector3.forward*UnityEngine.Random.value*360;

	}

}
