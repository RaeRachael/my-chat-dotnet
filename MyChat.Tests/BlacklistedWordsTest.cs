﻿using System.IO;
using System.Linq;
using MyChat;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;

namespace MindLink.Recruitment.MyChat.Tests
{
    using System;

    /// <summary>
    /// Tests for the <see cref="ConversationExporter"/>.
    /// </summary>
    [TestFixture]
    public class BlackListedWordsTests
    {
        /// <summary>
        /// Tests that exporting the conversation exports conversation.
        /// </summary>
        [Test]
        public void ExportingConversationWithBlacklistedWord()
        {
             var exporter = new ConversationExporter();
            string[] args = { "--blacklist", "pie" };
            var editorCofig = new EditingConfiguration(args);
            var editor = new ConversationEditor(editorCofig);

            exporter.ExportConversation("chat.txt", "chatBlacklist.json", editor);

            var serializedConversation = new StreamReader(new FileStream("chatBlacklist.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[1].senderId, Is.EqualTo("mike"));
            Assert.That(messages[1].content, Is.EqualTo("how are you?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("I'm good thanks, do you like *redacted*?"));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].senderId, Is.EqualTo("mike"));
            Assert.That(messages[3].content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(messages[4].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].senderId, Is.EqualTo("angus"));
            Assert.That(messages[4].content, Is.EqualTo("Hell yes! Are we buying some *redacted*?"));

            Assert.That(messages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].senderId, Is.EqualTo("bob"));
            Assert.That(messages[5].content, Is.EqualTo("No, just want to know if there's anybody else in the *redacted* society..."));

            Assert.That(messages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].senderId, Is.EqualTo("angus"));
            Assert.That(messages[6].content, Is.EqualTo("YES! I'm the head *redacted* eater there..."));
        }

        [Test]
        public void ExportingConversationWithMultipleBlacklistedWords()
        {
            var exporter = new ConversationExporter();
            string[] args = { "--blacklist", "buying,pie" };
            var editorCofig = new EditingConfiguration(args);
            var editor = new ConversationEditor(editorCofig);

            exporter.ExportConversation("chat.txt", "chatBlacklist2.json", editor);

            var serializedConversation = new StreamReader(new FileStream("chatBlacklist2.json", FileMode.Open)).ReadToEnd();

            var savedConversation = JsonConvert.DeserializeObject<Conversation>(serializedConversation);

            Assert.That(savedConversation.name, Is.EqualTo("My Conversation"));

            var messages = savedConversation.messages.ToList();

            Assert.That(messages[0].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470901)));
            Assert.That(messages[0].senderId, Is.EqualTo("bob"));
            Assert.That(messages[0].content, Is.EqualTo("Hello there!"));

            Assert.That(messages[1].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470905)));
            Assert.That(messages[1].senderId, Is.EqualTo("mike"));
            Assert.That(messages[1].content, Is.EqualTo("how are you?"));

            Assert.That(messages[2].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470906)));
            Assert.That(messages[2].senderId, Is.EqualTo("bob"));
            Assert.That(messages[2].content, Is.EqualTo("I'm good thanks, do you like *redacted*?"));

            Assert.That(messages[3].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470910)));
            Assert.That(messages[3].senderId, Is.EqualTo("mike"));
            Assert.That(messages[3].content, Is.EqualTo("no, let me ask Angus..."));

            Assert.That(messages[4].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470912)));
            Assert.That(messages[4].senderId, Is.EqualTo("angus"));
            Assert.That(messages[4].content, Is.EqualTo("Hell yes! Are we *redacted* some *redacted*?"));

            Assert.That(messages[5].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470914)));
            Assert.That(messages[5].senderId, Is.EqualTo("bob"));
            Assert.That(messages[5].content, Is.EqualTo("No, just want to know if there's anybody else in the *redacted* society..."));

            Assert.That(messages[6].timestamp, Is.EqualTo(DateTimeOffset.FromUnixTimeSeconds(1448470915)));
            Assert.That(messages[6].senderId, Is.EqualTo("angus"));
            Assert.That(messages[6].content, Is.EqualTo("YES! I'm the head *redacted* eater there..."));
        }

        [Test]
        public void OnlyRedactCompleteWordsRedactBlacklistedWords()
        {   
            string[] args = { "--blacklist", "i" };
            var editorCofig = new EditingConfiguration(args);
            var editor = new ConversationEditor(editorCofig);
            var message = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "name", "i tested string");
            
            editor.ApplyRegexRedaction(message);
            Assert.That(message.content, Is.EqualTo("*redacted* tested string"));
        }

        [Test]
        public void RedactCompleteWordsNextToPunctuationRedactBlacklistedWords()
        {
            string[] args = { "--blacklist", "string" };
            var editorCofig = new EditingConfiguration(args);
            var editor = new ConversationEditor(editorCofig);
            var message = new Message(DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64("1448470901")), "name", "tested string!");
            
            editor.ApplyRegexRedaction(message);
            Assert.That(message.content, Is.EqualTo("tested *redacted*!"));
        }
    }
}
