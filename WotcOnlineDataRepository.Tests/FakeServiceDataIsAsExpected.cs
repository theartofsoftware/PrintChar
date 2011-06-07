﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NUnit.Framework;

namespace WotcOnlineDataRepository.Tests
{
	[TestFixture]
	public class FakeServiceDataIsAsExpected
	{
		[Test]
		public void MonkPowerIsRight()
		{
			const string expectedPowerDetails =
				@"<div id=""detail"">
		
		<h1 class=""atwillpower""><span class=""level"">Monk Feature </span>Centered Flurry of Blows</h1><p class=""flavor""><i>Your fists become a blur as you follow up your initial attack with another, shifting your foes’ positions to your advantage.</i></p><p class=""powerstat""><b>At-Will</b>&nbsp;&nbsp; <img src=""images/bullet.gif"" alt="""">&nbsp;&nbsp;&nbsp;&nbsp; <b>Psionic</b><br><b>Free Action (Special)</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Melee</b> 1</p><p class=""powerstat""><b>Trigger</b>: You hit with an attack during your turn</p><p class=""powerstat""><b>Target</b>: One creature<br>Level 11: One or two creatures<br>Level 21: Each enemy adjacent to you</p><p class=""flavor""><b>Effect</b>: The target takes damage equal to 2 + your Wisdom modifier, and you slide it 1 square to a square adjacent to you, or 1 square in any direction if the target wasn’t targeted by the triggering attack.</p><p class=""powerstat""><b>Special</b>: You can use this power only once per round.</p><br><p>Published in <a href=""http://www.wizards.com/default.asp?x=products/dndlist&brand=All&year=All&type=CoreGameProducts"" target=""_new"">Player's Handbook 3</a>, page(s) 65.</p>
    </div>";
			Assert.That(_PowerDataFor(TestPowers.Monk).Result.WriteTo(), Is.EqualTo(expectedPowerDetails));
		}

		[Test]
		public void ArdentPowerIsRight()
		{
			const string expectedPowerDetails =
				@"<div id=""detail"">
		
		<h1 class=""atwillpower""><span class=""level"">Ardent Attack 3</span>Unsteadying Rebuke</h1><p class=""flavor""><i>Your enemy’s attack inspires a savage psionic rebuke, letting you shift the battlefield’s perspective in your favor.</i></p><p class=""powerstat""><b>At-Will</b>&nbsp;&nbsp; <img src=""images/bullet.gif"" alt="""">&nbsp;&nbsp;&nbsp;&nbsp; <b>Augmentable</b>, <b>Psionic</b>, <b>Weapon</b><br><b>Immediate Reaction</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Melee</b> weapon</p><p class=""powerstat""><b>Trigger</b>: An enemy targets you with a melee attack</p><p class=""powerstat""><b>Target</b>: The triggering enemy</p><p class=""powerstat""><b>Attack</b>: Charisma vs. AC</p><p class=""flavor""><b>Hit</b>: 1[W] + Charisma modifier damage, and you slide the target 1 square to a square adjacent to you.</p><p class=""powerstat""><b>Effect</b>: You lose your standard action on your next turn.</p><b>Augment 1</b><br><p class=""flavor""><b>Hit</b>: As above, and one ally adjacent to you can shift to any unoccupied square adjacent to the target’s new position as a free action.</p><b>Augment 2</b><br><p class=""powerstat""><b>Hit</b>: 1[W] + Charisma modifier damage, and you slide the target a number of squares equal to your Wisdom modifier. You then slide each enemy now adjacent to the target 1 square.</p><p class=""flavor""><b>Effect</b>: You do not lose your standard action on your next turn.</p><br><p>Published in <a href=""http://www.wizards.com/dnd/Product.aspx?x=dnd/products/dndacc/210940000"" target=""_new"">Psionic Power</a>, page(s) 13.</p>
    </div>";
			Assert.That(_PowerDataFor(TestPowers.Ardent).Result.WriteTo(), Is.EqualTo(expectedPowerDetails));
		}

		[Test]
		public void PowerWithSubpowerIsRight()
		{
			const string expectedPowerDetails =
				@"<div id=""detail"">
		
		<h1 class=""encounterpower""><span class=""level"">Wizard Attack 3</span>Hypnotic Pattern</h1><p class=""flavor""><i>A swirling pattern of colors appears before your foes. Their eyes glaze over as the pattern enthralls them and lures them closer.</i></p><p class=""powerstat""><b>Encounter</b>&nbsp;&nbsp; <img src=""images/bullet.gif"" alt="""">&nbsp;&nbsp;&nbsp;&nbsp; <b>Arcane</b>, <b>Illusion</b>, <b>Implement</b><br><b>Standard Action</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Ranged</b> 10</p><p class=""flavor""><b>Effect</b>: You conjure a swirling pattern of colors and lights in an unoccupied square within range that lasts until the end of your next turn. You can use the Hypnotic Pattern Attack power, using the pattern’s square as the origin square.</p><br><h1 class=""atwillpower""><span class=""level""></span>Hypnotic Pattern Attack</h1><p class=""flavor""><i>A swirling pattern of colors appears before your foes. Their eyes glaze over as the pattern enthralls them and lures them closer.</i></p><p class=""powerstat""><b>At-Will</b> <img src=""images/bullet.gif"" alt="""">&nbsp;&nbsp;&nbsp;&nbsp; <b>Arcane</b>, <b>Conjuration</b>, <b>Illusion</b>, <b>Implement</b><br><b>Opportunity Action</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Close</b> burst 3</p><p class=""flavor""><b>Requirement</b>: The Hypnotic Pattern power must be active to use this power.</p><p class=""powerstat""><b>Trigger</b>: An enemy starts its turn within 3 squares of the pattern</p><p class=""powerstat""><b>Target</b>: The triggering enemy in the burst</p><p class=""powerstat""><b>Attack</b>: Intelligence vs. Will</p><p class=""flavor""><b>Hit</b>: The target is pulled 3 squares toward the pattern and is slowed until the end of your next turn. It can move into the pattern’s square.</p><br>
    </div>";
			Assert.That(_PowerDataFor(TestPowers.PowerWithSubpower).Result.WriteTo(), Is.EqualTo(expectedPowerDetails));
		}

		[Test]
		public void RequestingSubpowerFailsCorrectly()
		{
			var request = _PowerDataFor(TestPowers.Subpower);
			var aggregateException = Assert.Throws<AggregateException>(() => { var foo = request.Result; });
			Assert.That(aggregateException.Flatten().InnerExceptions.Select(ex => ex.GetType()), Is.EqualTo(new[] {typeof (WebException)}));
		}

		private static Task<HtmlNode> _PowerDataFor(TestPowers whichPower)
		{
			return ServiceFactory.MakeLocalOnlyFakeServiceForTesting().Result.Raw.PowerData(whichPower);
		}
	}
}
