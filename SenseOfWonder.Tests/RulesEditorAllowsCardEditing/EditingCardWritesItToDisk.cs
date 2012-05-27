﻿using System;
using EventBasedProgramming.Binding;
using EventBasedProgramming.TestSupport;
using FluentAssertions.Assertions;
using FluentAssertions.EventMonitoring;
using NUnit.Framework;
using SenseOfWonder.Model;
using SenseOfWonder.Model.Serialization;
using SenseOfWonder.Tests.zzTestSupportData;
using System.Linq;
using FluentAssertions;

namespace SenseOfWonder.Tests.RulesEditorAllowsCardEditing
{
	[TestFixture]
	public class EditingCardWritesItToDisk
	{
		[Test]
		public void CardShouldNotifyObserversWhenNameIsChanged()
		{
			var testSubject = new CardData
			{
				Name = "initial name"
			};
			testSubject.MonitorEvents();
			testSubject.Name = "new value";
			testSubject.ShouldRaisePropertyChangeFor(s => s.Name);
		}

		[Test, RequiresSTA]
		public void RulesEditorShouldNotifyThatPersistableDataHasChangedWhenAnyCardDataChanges()
		{
			var testSubject = _TestData.EmptyRulesetCharacter();
			testSubject.Name = "new card name";
			testSubject.CreateCard();
			PropagateChangesTo(testSubject.CardData.First().Should(), () => testSubject.PersistableData);
		}

		private void PropagateChangesTo(ObjectAssertions changeNotifier, Func<object> propertyThatShouldChange)
		{
			changeNotifier.BeAssignableTo<IFirePropertyChanged>();
			//changeNotifier.As<IFirePropertyChanged>().PropertyChanged += sf
		}
	}
}