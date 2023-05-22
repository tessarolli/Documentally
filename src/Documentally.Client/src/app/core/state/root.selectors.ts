import { AppState } from '../../app.state';

export const selectPageTitle = (state: AppState) => state?.root.pageTitle;
