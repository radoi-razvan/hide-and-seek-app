export const htmlFactory = {
  errorResponse: (error) => {
    return `
    <p class="text-danger">${error}</p>
    <button id="reloadBtn" class="btn form-btn">Try Again</button>
    `;
  },
  successResponse: (message) => {
    return `
        <p class="text-success">${message}</p>
        <form id="downloadForm">
            <div class="form-group row mt-2 mb-2">
              <label for="newFileName" class="col-sm-2 col-form-label app-logo">
                New File Name
              </label>
              <div class="col-sm-10">
                <input
                  type="text"
                  class="form-control"
                  name="newFileName"
                  id="newFileName"
                  required
                />
              </div>
            </div>
            <div class="form-group row">
              <div class="col-sm-10">
                <button id="setFileNameBtn" type="submit" class="btn form-btn">
                  Set File Name
                </button>
              </div>
            </div>
          </form>
        `;
  },
  downloadLink: (downloadData) => {
    return `<a id="downloadLink" href="${process.env.REACT_APP_BASE_URL_BACKEND}/cryptography/download/${downloadData.oldFileName}/${downloadData.newFileName}/${downloadData.operation}" class="btn form-btn" download>Download</a>`;
  },
  reloadButton: () => {
    return `<button id="reloadBtn" class="btn form-btn">Try Again</button>`;
  },
};

{
  /* <form id="downloadForm">
  <div class="form-group row mt-2 mb-2">
    <label for="newFileName" class="col-sm-2 col-form-label app-logo">
      New File Name
    </label>
    <div class="col-sm-10">
      <input
        type="text"
        class="form-control"
        name="newFileName"
        id="newFileName"
        required
      />
    </div>
  </div>
  <div class="form-group row">
    <div class="col-sm-10">
      <button type="submit" class="btn form-btn">
        Download
      </button>
    </div>
  </div>
</form> */
}
