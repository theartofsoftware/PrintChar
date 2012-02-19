﻿using System.IO;
using FluentAssertions;
using NUnit.Framework;
using PluginApi.Model;

namespace PluginApi.Tests
{
	[TestFixture]
	public class DescribeAGameSystem
	{
		[Test]
		public void ShouldHaveAName()
		{
			_testSubject.Name.Should().Be("4th Edition D&D");
		}

		[Test]
		public void ShouldDescribeFileFilters()
		{
			_testSubject.FileExtension.Should().Be("dnd4e");
			_testSubject.FilePattern.Should().Be("*.dnd4e");
		}

		[Test]
		public void ShouldUseLabelAndExtensionCorrectlyToInitializeOpenDialog()
		{
			_testSubject.CreateOpenDialog().ShouldHave().SharedProperties().EqualTo(new
			{
				Filter = "4th Edition D&D file (*.dnd4e)|*.dnd4e",
				DefaultExt = "dnd4e",
				CheckFileExists = true,
				Multiselect = false,
				InitialDirectory = string.Empty
			});
		}

		[Test]
		public void IfHaveAlreadyOpenedCharacterShouldSetOpenDialogToStartInThatLocation()
		{
			_testSubject.Character = _arbitraryCharacter;
			_testSubject.CreateOpenDialog().InitialDirectory.Should().Be(_arbitraryCharacter.File.Location.DirectoryName);
		}

		[Test]
		public void CancellingTheOpenDialogShouldResultInNoChangeInOpenCharacter()
		{
			_testSubject.Character = _arbitraryCharacter;
			_testSubject.LoadCharacter(null);
			_testSubject.Character.Should().BeSameAs(_arbitraryCharacter);
		}

		[Test]
		public void OpeningANewCharacterShouldUpdateTheCharacterFile()
		{
			string tempFile = Path.GetTempFileName();
			using (Undo.Step(() => File.Delete(tempFile)))
			{
				_testSubject.Character = _arbitraryCharacter;
				_testSubject.LoadCharacter(tempFile);
				_testSubject.Character.File.Location.FullName.Should().Be(tempFile);
			}
		}

		[Test]
		public void ShouldAllowCreatingNewCharactersByDefault()
		{
			_testSubject.IsReadOnly.Should().BeFalse();
		}

		[Test]
		public void ShouldAllowGameSystemsWhichCannotCreateCharacters()
		{
			_testSubject.IsReadOnly = true;
			_testSubject.IsReadOnly.Should().BeTrue();
		}

		private GameSystem _testSubject;
		private Character _arbitraryCharacter;

		[SetUp]
		public void Setup()
		{
			_testSubject = new _SimplisticGameSystem();
			_arbitraryCharacter = new SillyCharacter(new CachedFile(new FileInfo(Path.GetTempPath() + @"ee.dnd4e"), false));
		}

		public class SillyCharacter : Character
		{
			public SillyCharacter(CachedFile data)
			{
				File = data;
			}
		}

		internal class _SimplisticGameSystem : GameSystem
		{
			public _SimplisticGameSystem()
				: base("4th Edition D&D", "dnd4e") {}

			protected override Character Parse(CachedFile characterData)
			{
				return new SillyCharacter(characterData);
			}
		}
	}
}
