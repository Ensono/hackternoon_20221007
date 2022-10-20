# Slacktistics

Slacktistics is a Slack bot that posts statistics about your Slack team to a channel of your choice.

Running `app.py` will retrieve the message history of the `#hackternoon-team5` Slack channel, 
read the result into a Pandas dataframe, and count how many messages were sent by each user.
It then posts a message back to the `#hackternoon-team5` channel with the user who sent the most messages
in the channel.

This is very much a POC, and there are many things that can be done with the data available via the Slack API.