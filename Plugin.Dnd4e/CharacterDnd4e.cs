﻿using System.Collections.ObjectModel;
using System.Windows;
using JetBrains.Annotations;
using PluginApi.Display.Helpers;
using PluginApi.Model;

namespace Plugin.Dnd4e
{
	public class CharacterDnd4E : Character<GameSystem4E>
	{
		[NotNull] private readonly TrackingNonNullProperty<string> _race;
		[NotNull] private readonly TrackingNonNullProperty<string> _charClass;
		[NotNull] private readonly ObservableCollection<Power> _powers = new ObservableCollection<Power>();
		[NotNull] private readonly IDataFile _configFile;

		public CharacterDnd4E([NotNull] GameSystem4E system, [NotNull] IDataFile configFile) : base(system)
		{
			_configFile = configFile;
			_gender.AddDependantProperty(() => SummaryLine);
			_race = new TrackingNonNullProperty<string>(string.Empty, this, () => Race, () => SummaryLine);
			_charClass = new TrackingNonNullProperty<string>(string.Empty, this, () => CharClass, () => SummaryLine);
			EqualityFields.Add(_race, _charClass);
			EqualityFields.AddSequences(_powers);
		}

		[NotNull]
		public string SummaryLine
		{
			get { return string.Format("{0} {1} {2}", _gender, _race, _charClass); }
		}

		[NotNull]
		public ObservableCollection<Power> Powers
		{
			get { return _powers; }
		}

		[NotNull]
		public string Race
		{
			get { return _race.Value; }
			set { _race.Value = value; }
		}

		[NotNull]
		public string CharClass
		{
			get { return _charClass.Value; }
			set { _charClass.Value = value; }
		}

		public string ConfigData
		{
			get { return _configFile.Contents; }
			set
			{
				if (_configFile.Contents == value)
					return;
				_configFile.Contents = value;
				FirePropertyChanged(() => ConfigData);
			}
		}

		public override string ToString()
		{
			return string.Format("Character(Name: {0}, Gender: {2}, Powers: [{1}])", _name, string.Join(", ", _powers), _gender);
		}
	}

	public class SamThe4ECharacter : CharacterDnd4E
	{
		public SamThe4ECharacter() : base(new GameSystem4E())
		{
			Name = "Sam Ple O'data";
			Gender = "Male";
			Race = "Gnome";
			CharClass = "Illusionist";
		}
	}
}
