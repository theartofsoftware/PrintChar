﻿using FluentAssertions;
using JetBrains.Annotations;
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
			_testSubject.Name.Should().Be("Trivial");
		}

		[Test]
		public void ShouldDescribeFileFilters()
		{
			_testSubject.FileExtension.Should().Be("simple");
			_testSubject.FilePattern.Should().Be("*.simple");
		}

		[Test]
		public void ShouldLoadCharactersFromFilesOnCommand()
		{
			_testSubject.LoadCharacter(ArbitraryFile).File.Location.FullName.Should().Be(ArbitraryFile);
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

		private _SimplisticGameSystem _testSubject;

		[SetUp]
		public void Setup()
		{
			_testSubject = new _SimplisticGameSystem();
		}

		private const string ArbitraryFile = @"C:\irrelevant\file.name";

		public class SillyCharacter : Character<GameSystem>
		{
			public SillyCharacter([NotNull] IDataFile data, [NotNull] GameSystem system) : base(system)
			{
				File = data;
			}
		}
	}
}
