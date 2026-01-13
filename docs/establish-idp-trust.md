# Establish IdP Trust

This document outlines the steps to set up Zitadel with GitHub as an Identity Provider (IdP).

## Prerequisites

- Admin access to your GitHub organization or personal account
- Admin access to your Zitadel instance
- Your Zitadel instance must be running and accessible

## Register a new GitHub OAuth App

1. Navigate to GitHub Settings:
   - For personal accounts: Go to [GitHub Settings → Developer settings → OAuth Apps](https://github.com/settings/developers)
   - For organizations: Go to your organization → Settings → Developer settings → OAuth Apps

2. Click **"New OAuth App"** button

3. Configure the following fields:

   - **Application Name**: This can be whatever you want (e.g., "Fullhouse Platform" or "Zitadel Integration").
   - **Homepage URL**: This is the "contact page" linked to users when they review their consent with their IdP. Use your application's public URL (e.g., `https://yourdomain.com`).
   - **Authorization Callback URI**: This is where GitHub will redirect users after authentication. Use Zitadel's callback URL:
     - For local development: `http://localhost:3000/idps/callback`
     - For production: Replace `localhost:3000` with your hosting domain (e.g., `https://yourdomain.com/idps/callback`)
     - **Important**: This URL must match exactly what's configured in Zitadel, including the protocol (`http://` vs `https://`)

4. Click **"Register application"**

5. After registration, you'll be shown:
   - **Client ID**: Copy this value (you'll need it for Zitadel configuration)
   - **Client Secret**: Click "Generate a new client secret" and copy it immediately (you won't be able to see it again). Store this securely.

## Configure Zitadel Side Connection

After registering the GitHub OAuth App, configure the connection in Zitadel:

1. Log in to your Zitadel instance (typically at `http://localhost:3000` for local development)

2. Navigate to **Settings** → **Identity Providers** (or **IdPs**)

3. Click **"New Provider"** or **"Add Provider"** and select **GitHub** (or **OAuth** if GitHub isn't listed directly)

4. Fill in the configuration fields:

   - **Name**: Give this provider a descriptive name (e.g., "GitHub")
   - **Client ID**: Paste the Client ID you copied from GitHub. This is not sensitive and is designed to be public.
   - **Client Secret**: Paste the Client Secret you copied from GitHub. **Keep this secret secure** - treat it like a password.
   - **Scopes**: Configure the OAuth scopes you need (typically `read:user` and `user:email` for basic user information)

5. Save the configuration

6. **Important**: Ensure the callback URL in Zitadel matches exactly what you configured in GitHub (including protocol and port)

## Verification and Testing

After completing the setup, verify the integration works:

1. **Test the connection**:
   - Attempt to log in using GitHub as the identity provider

2. **Common issues to check**:
   - **Callback URL mismatch**: The callback URL in GitHub must exactly match what Zitadel expects (check for `http://` vs `https://`, trailing slashes, and port numbers)
   - **Client Secret**: Ensure you copied the entire secret without extra spaces or line breaks
   - **Network access**: Ensure your Zitadel instance can reach GitHub's OAuth endpoints
   - **Scopes**: Verify the requested scopes are appropriate and don't require additional GitHub app permissions

3. **Expected behavior**:
   - When users log in, they should see a GitHub authorization screen
   - After authorizing, they should be redirected back to your application
   - User information from GitHub should be available in Zitadel

## Risk Analysis - Worst Case Scenario

### Prerequisites (both must be true):
- **Client Secret Leaked**
- **Callback URI Hijacked**

### Fallout:
- Malicious actor can impersonate scope requests from our service
- **IF** user consents to these requests, malicious actor gains access
- Malicious actor has access to these resources for the lifetime of the token or until revoked
- Limited to users who consented

### Rectification:
- Revoke/Regain Callback URI
- Revocation of all user tokens
- All users must sign in again

## Additional Notes

- **Multiple environments**: If you have separate GitHub OAuth Apps for development, staging, and production, you'll need separate IdP configurations in Zitadel for each environment.

- **Updating the Client Secret**: If you need to regenerate the Client Secret in GitHub, you'll need to update it in Zitadel as well. The old secret will stop working immediately.

- **Callback URL for different environments**: Remember to update the callback URL in GitHub when deploying to different environments (development, staging, production).

- **Security best practices**:
  - Never commit Client Secrets to version control
  - Use environment variables or secret management systems to store Client Secrets
  - Regularly rotate Client Secrets as part of your security practices
  - Monitor for unauthorized access attempts
