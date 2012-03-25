﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using EventBasedProgramming.Binding;
using JetBrains.Annotations;
using Microsoft.Win32;
using PluginApi.Model;
using PrintChar.DesignTimeSupportData;

namespace PrintChar
{
	public class AllGameSystemsViewModel : IFirePropertyChanged
	{
		[NotNull] private readonly GameSystem[] _allGameSystems;
		[NotNull] private readonly TrackingNullableProperty<Character> _currentCharacter;
		private readonly CharacterFileInteraction _openExistingCharacter;
		private readonly CharacterFileInteraction _createNewCharacter;

		public AllGameSystemsViewModel() : this(Plugins.GameSystems()) {}

		public AllGameSystemsViewModel([NotNull] params GameSystem[] gameSystems)
		{
			_allGameSystems = gameSystems;
			_openExistingCharacter = new CharacterFileInteraction(_allGameSystems, true,
				(gameSystem, fileName) => gameSystem.LoadCharacter(fileName));
			_createNewCharacter = new CharacterFileInteraction(_allGameSystems.Where(g => !g.IsReadOnly), false,
				(gameSystem, fileName) => gameSystem.LoadCharacter(fileName));
			_currentCharacter = new TrackingNullableProperty<Character>(this,
				() => Character, () => IsValid, () => CharFileName);
			OpenCharCommand = new SimpleCommand(() => true, SwitchCharacter);
			CreateCharCommand = new SimpleCommand(_HasAtLeastOneWritableGameSystem, CreateNewCharacter);
		}

		public SimpleCommand OpenCharCommand { get; private set; }
		public SimpleCommand CreateCharCommand { get; private set; }

		public void SwitchCharacter()
		{
			Character = _openExistingCharacter.LoadCharacter(Character);
		}

		public void CreateNewCharacter()
		{
			Character = _createNewCharacter.LoadCharacter(Character);
		}

		[NotNull]
		public OpenFileDialog CreateOpenDialog()
		{
			return _openExistingCharacter.CreateDialog(Character);
		}

		[NotNull]
		public OpenFileDialog CreateCreateDialog()
		{
			return _createNewCharacter.CreateDialog(Character);
		}

		[CanBeNull]
		public Character Character
		{
			get { return _currentCharacter.Value; }
			protected set { _currentCharacter.Value = value; }
		}

		public bool IsValid
		{
			get { return _currentCharacter.Value != null; }
		}

		[NotNull]
		public string CharFileName
		{
			get { return _currentCharacter.Value == null ? string.Empty : _currentCharacter.Value.File.Location.FullName; }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void FirePropertyChanged(Expression<Func<object>> propertyThatChanged)
		{
			PropertyChanged.Raise(this, propertyThatChanged);
		}

		private bool _HasAtLeastOneWritableGameSystem()
		{
			return _allGameSystems.FirstOrDefault(gs => gs.IsReadOnly == false) != null;
		}
	}

	public class DesignTimeGameSystems : AllGameSystemsViewModel
	{
		private readonly DesignTimeGameSystem _gameSystem;

		public DesignTimeGameSystems() : this(new DesignTimeGameSystem())
		{
			Character = new TheDesigner(_gameSystem);
		}

		private DesignTimeGameSystems(DesignTimeGameSystem gameSystem) : base(gameSystem)
		{
			_gameSystem = gameSystem;
		}
	}
}
