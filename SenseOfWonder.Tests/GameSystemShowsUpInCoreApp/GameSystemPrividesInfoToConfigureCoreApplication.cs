﻿using FluentAssertions;
using NUnit.Framework;

namespace SenseOfWonder.Tests.GameSystemShowsUpInCoreApp
{
	[TestFixture]
	public class GameSystemPrividesInfoToConfigureCoreApplication
	{
		[Test]
		public void ShouldAllowCharacterCreationAndEditing()
		{
			new SenseOfWonderSystem().IsReadOnly.Should().BeFalse();
		}

		[Test]
		public void ShouldProvideFileFilterInfo()
		{
			new SenseOfWonderSystem().ShouldHave().SharedProperties().EqualTo(new
			{
				FileExtension = "wonder",
				Name = "Sense of Wonder",
				FilePattern = "*.wonder"
			});
		}
	}
}
