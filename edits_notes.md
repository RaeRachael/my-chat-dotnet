# Comments

## reading command line arguments
### making my own version instead of using previous one
* reading of the arguments into string[] leads to difficulties in arguement checking
* reading into an editorConfig instead of a global config doesn't feel nice
  espically when it is also logCreatorConfig
### One Config or SpecificConfig for each object...
* personally I would prefer to see one config - where the only the needed properties were 
  accessed by the objects it is passed into during construction,
* but I can see for slighly larger projects individual configs would be nicer. 
  (each one being made during a single read of the CL args)

## conversationReader - logWriter
### conversationReader takes a filePath in construction - logWriter doesn't
* feels inconsistant
* might be nice to have a constructor for a filePath and one without a filePath for both..
  with correspoding read/write functions that need a filePath or throw exception

## ConversationLog 
### could have been an extention to the Conversation class
* instead of LogWithReport just make it ConversationWithReport
* would save a class type being made and a Log object being made is no report is needed

* would possibly make adapting the output style harder in the future - as Conversation as a 
  class would then be holding data to be changed (as needed) and the fields for the output
  [same object two responsiblities...]

## ExportConversation ( )
### taking strings as parementers for filePaths, now feels strange.
* I would like them to be already in objects or the config passed directly... (maybe..)

## Filters
### abstract class with abstract Appyfilter method would be better
* As there is no intention to make a parent Filter instance ever
### seperate file for each of the children would fit C# convention better

### Language Integrated Query for filters...? 
* Alternative code, also saves making a new varible long term.

``` 
  var editedMessages = new List<Message>();
    foreach(Message message in conversation.messages)
    {
        if (message.content.Contains(this.keywordfilter))
        {
            editedMessages.Add(message);
        }
    };
    conversation.messages = editedMessages;

  conversation.messages = from message in conversation
                          where message.content.Contains(this.keywordfilter)
                          select message

  conversation.messages =
    conversation.messages.where(m => m.content.Contains(this.keywordfilter))
```

