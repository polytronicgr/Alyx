﻿using System;
using System.Collections.Generic;

namespace Alyx {
	public class Sentence {

		public Word[] words;
		public string[] tags;

		//Creates a new sentence of seperate known words and frequent tags from a given phrase
		public Sentence (string phrase) {
			words = individualWords (reformat (phrase));
			selectCommonTags (setTagFrequency ());
			foreach (Word term in words)
				Console.Write ("{0} ", term.name);
			Console.WriteLine ("\n{0} {1} {2}", tags [0], tags [1], tags [2]);
		}

		//Reformats the words in the phrase string
		//    Switches capitals so the computer can read
		//    Removes illegal characters
		public string reformat (string phrase) {
			string ABC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string abc = "abcdefghijklmnopqrstuvwxyz";
			string illegal = "@#$%^&*()+=-|{}[]/><~_";
			string formatted = "";
			for (int i = 0; i < phrase.Length; i++) {
				if (i == 0 || phrase [i - 1] == ' ') {
					if (abc.Contains (phrase [i].ToString ()))
						formatted += ABC [abc.IndexOf (phrase [i])];
					else if (!illegal.Contains (phrase [i].ToString ()))
						formatted += phrase [i];
				} else if (ABC.Contains (phrase [i].ToString ()))
					formatted += abc [ABC.IndexOf (phrase [i])];
				else if (!illegal.Contains (phrase [i].ToString ()))
					formatted += phrase [i];
			}
			return formatted;
		}

		//Seperates the phrase into a collection of known words.
		//Ignores unknown words.
		public Word[] individualWords (string phrase) {
			List<Word> terms = new List<Word> ();
			string nonWordChars = " ,.?:;!";
			string substring = "";
			for (int i = 0; i < phrase.Length; i++) {
				if (nonWordChars.Contains (phrase [i].ToString ())) {
					Word newTerm = Word.fromCollection (substring, Program.vocab.ToArray ());
					if (newTerm != null)
						terms.Add (newTerm);
					substring = "";
				} else if (i == phrase.Length - 1) {
					substring += phrase [i];
					Word newTerm = Word.fromCollection (substring, Program.vocab.ToArray ());
					if (newTerm != null)
						terms.Add (newTerm);
					substring = "";
				} else
					substring += phrase [i];
			}
			return terms.ToArray ();
		}

		//Determines the frequency of tags showing up in the words array
		public Dictionary<string, int> setTagFrequency () {
			Dictionary <string, int> tagCounter = new Dictionary<string, int> ();
			foreach (Word word in words) {
				string substring = "";
				for (int i = 0; i < word.tags.Length; i++) {
					if (word.tags [i] == ' ') {
						if (tagCounter.ContainsKey (substring))
							tagCounter [substring]++;
						else
							tagCounter.Add (substring, 1);
						substring = "";
					} else if (i == word.tags.Length - 1) {
						substring += word.tags [i];
						if (tagCounter.ContainsKey (substring))
							tagCounter [substring]++;
						else
							tagCounter.Add (substring, 1);
						substring = "";
					} else
						substring += word.tags [i];
				}
			}
			return tagCounter;
		}

		//Chooses the top three most frequent tags and sets them as this sentences tag collection
		public void selectCommonTags (Dictionary<string, int> frequencies) {
			frequencies.Remove ("");
			int firstFrequency = 0;
			int secondFrequency = 0;
			int thirdFrequency = 0;
			string first = "";
			string second = "";
			string third = "";
			foreach (string tag in frequencies.Keys) {
				if (frequencies [tag] >= firstFrequency) {
					thirdFrequency = secondFrequency;
					secondFrequency = firstFrequency;
					firstFrequency = frequencies [tag];
					third = second;
					second = first;
					first = tag;
				} else if (frequencies [tag] >= secondFrequency) {
					thirdFrequency = secondFrequency;
					secondFrequency = frequencies [tag];
					third = second;
					second = tag;
				} else {
					thirdFrequency = frequencies [tag];
					third = tag;
				}
			}
			tags = new string[] { first, second, third };
		}

	}
}