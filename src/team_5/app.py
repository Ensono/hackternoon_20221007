import os
import logging
# Use the package we installed
# from slack_bolt import App
# Import WebClient from Python SDK (github.com/slackapi/python-slack-sdk)
from slack_sdk import WebClient
from slack_sdk.errors import SlackApiError
import pandas as pd

# Initializes your app with your bot token and signing secret
# app = App(
#     token=os.environ.get("SLACK_BOT_TOKEN"),
#     signing_secret=os.environ.get("SLACK_SIGNING_SECRET")
# )

token = os.environ.get("SLACK_BOT_TOKEN")
# signing_secret = os.environ.get("SLACK_SIGNING_SECRET")

# Add functionality here
# @app.event("app_home_opened") etc


# WebClient instantiates a client that can call API methods
# When using Bolt, you can use either `app.client` or the `client` passed to listeners.
client = WebClient(token=token)
logger = logging.getLogger(__name__)

# Store conversation history
conversation_history = []
# ID of the channel you want to send the message to
channel_id = "C045L7XFT60"

try:
    # Call the conversations.history method using the WebClient
    # conversations.history returns the first 100 messages by default
    # These results are paginated, see: https://api.slack.com/methods/conversations.history$pagination
    result = client.conversations_history(channel=channel_id)

    conversation_history = result["messages"]

    # Print results
    logger.info("{} messages found in {}".format(len(conversation_history), id))

except SlackApiError as e:
    logger.error("Error creating conversation: {}".format(e))

df = pd.DataFrame(conversation_history)
agg = df.groupby(['user'], as_index=False).count()
user = agg['user'][0]
user_name = client.users_profile_get(user=user)['profile']['real_name']

client.chat_postMessage(channel=channel_id, text=f"Slacker of the month is <@{user}>!")

# Start your app
# if __name__ == "__main__":
#     app.start(port=int(os.environ.get("PORT", 3000)))
