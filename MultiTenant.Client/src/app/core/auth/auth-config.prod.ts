import { environment } from '@env/environment';
import { AuthConfig } from 'angular-oauth2-oidc';
import { setEnv } from './auth-module-config';

export const authProdConfig: AuthConfig = getObj();
function getObj(): AuthConfig {
  //   setEnv();
  return {
    issuer: environment.IssuerUri,
    clientId: 'IS4-Admin',
    requireHttps: environment.RequireHttps,
    redirectUri: environment.Uri + '/login-callback',
    scope: 'openid profile email jp_api.is4',
    responseType: 'code',
    silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',
    useSilentRefresh: true, // Needed for Code Flow to suggest using iframe-based refreshes
    sessionChecksEnabled: true,
    showDebugInformation: true, // Also requires enabling "Verbose" level in devtools
    clearHashAfterLogin: false, // https://github.com/manfredsteyer/angular-oauth2-oidc/issues/457#issuecomment-431807040,
    nonceStateSeparator: 'semicolon', // Real semicolon gets mangled by IdentityServer's URI encoding
  };
}
