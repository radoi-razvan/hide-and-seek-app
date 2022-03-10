import React, { useState } from "react";
import { dataManager } from "../dataManager";
import { htmlFactory } from "../htmlFactory";

export const Cryptography = () => {
  const handleSubmit = (e) => {
    e.preventDefault();

    const loader = (response) => {
      // Display result
      const resultDisplay = document.getElementById("result");
      resultDisplay.innerHTML = ``;

      if (response.status === 400) {
        resultDisplay.insertAdjacentHTML(
          `beforeend`,
          htmlFactory.errorResponse(response.data)
        );
        const reloadButton = document.getElementById("reloadBtn");
        reloadButton.addEventListener("click", (event) => {
          window.location.reload();
          event.preventDefault();
        });
      } else if (response.status === 201 || response.status === 200) {
        resultDisplay.insertAdjacentHTML(
          `beforeend`,
          htmlFactory.successResponse(response.data)
        );

        const uploadFileBtn = document.getElementById("uploadFileBtn");
        uploadFileBtn.disabled = true;

        // Add downloadForm
        const downloadForm = document.getElementById("downloadForm");
        const encryptInput = document.getElementById("exampleRadios1");
        const decryptInput = document.getElementById("exampleRadios2");
        downloadForm.addEventListener("submit", (event) => {
          const downloadData = {
            oldFileName:
              document.getElementById("file").files[
                parseInt(process.env.REACT_APP_FILE_INDEX)
              ].name,
            newFileName: event.target.newFileName.value,
            operation: encryptInput.checked
              ? encryptInput.value
              : decryptInput.value,
          };

          // Add downloadLink
          resultDisplay.innerHTML = ``;
          resultDisplay.insertAdjacentHTML(
            `beforeend`,
            htmlFactory.downloadLink(downloadData)
          );

          // Add reloadButton
          const downloadLink = document.getElementById("downloadLink");
          downloadLink.addEventListener("click", (e) => {
            resultDisplay.innerHTML = ``;
            resultDisplay.insertAdjacentHTML(
              `beforeend`,
              htmlFactory.reloadButton()
            );

            const reloadButton = document.getElementById("reloadBtn");
            reloadButton.addEventListener("click", (event) => {
              window.location.reload();
              event.preventDefault();
            });
            window.location.href = downloadLink.href;
            e.preventDefault();
          });
          event.preventDefault();
        });
      }
    };

    if (e.target.operation.value === "encrypt") {
      dataManager
        .postEncrypt({
          file: e.target.file.files[parseInt(process.env.REACT_APP_FILE_INDEX)],
          key: e.target.key.value,
          operation: e.target.operation.value,
          crc: e.target.crc.checked,
        })
        .then((response) => {
          loader(response);
        });
    } else if (e.target.operation.value === "decrypt") {
      dataManager
        .postDecrypt({
          file: e.target.file.files[parseInt(process.env.REACT_APP_FILE_INDEX)],
          key: e.target.key.value,
          operation: e.target.operation.value,
          crc: e.target.crc.checked,
        })
        .then((response) => {
          loader(response);
        });
    }
  };

  return (
    <>
      <div className="card">
        <div className="container">
          <form onSubmit={handleSubmit}>
            <div className="form-group row">
              <label
                htmlFor="file"
                className="col-sm-2 col-form-label app-logo"
              >
                File
              </label>
              <div className="col-sm-10">
                <input
                  onChange={() => {
                    const file = document.getElementById("file");
                    const key = document.getElementById("key");
                    const uploadFileBtn =
                      document.getElementById("uploadFileBtn");
                    if (file.value) {
                      if (key.value) {
                        uploadFileBtn.disabled = false;
                      }
                    }
                  }}
                  type="file"
                  className="form-control"
                  name="file"
                  id="file"
                  required
                />
              </div>
            </div>
            <div className="form-group row mt-2 mb-2">
              <label htmlFor="key" className="col-sm-2 col-form-label app-logo">
                Key
              </label>
              <div className="col-sm-10">
                <input
                  onChange={() => {
                    const file = document.getElementById("file");
                    const key = document.getElementById("key");
                    const uploadFileBtn =
                      document.getElementById("uploadFileBtn");
                    if (file.value) {
                      if (key.value) {
                        uploadFileBtn.disabled = false;
                      }
                    }
                  }}
                  type="text"
                  className="form-control"
                  name="key"
                  id="key"
                  placeholder="Hexadecimal Key"
                  pattern="[0-1]*"
                  required
                />
              </div>
            </div>
            <fieldset className="form-group">
              <div className="row">
                <legend className="col-form-label col-sm-2 pt-0 app-logo">
                  Operation
                </legend>
                <div className="col-sm-10">
                  <div className="form-check">
                    <input
                      onClick={() => {
                        const checkbox = document.getElementById("crc");
                        checkbox.disabled = false;
                      }}
                      className="form-check-input"
                      type="radio"
                      name="operation"
                      id="exampleRadios1"
                      value="encrypt"
                      required
                    />
                    <label
                      className="form-check-label"
                      htmlFor="exampleRadios1"
                    >
                      Encrypt
                    </label>
                  </div>
                  <div className="form-check">
                    <input
                      onClick={() => {
                        const checkbox = document.getElementById("crc");
                        checkbox.disabled = true;
                      }}
                      className="form-check-input"
                      type="radio"
                      name="operation"
                      id="exampleRadios2"
                      value="decrypt"
                      required
                    />
                    <label
                      className="form-check-label"
                      htmlFor="exampleRadios2"
                    >
                      Decrypt
                    </label>
                  </div>
                </div>
              </div>
            </fieldset>
            <div className="form-group row">
              <div className="col-sm-2 app-logo">Add CRC</div>
              <div className="col-sm-10">
                <div className="form-check">
                  <input
                    className="form-check-input"
                    type="checkbox"
                    id="crc"
                    name="crc"
                  />
                  <label className="form-check-label" htmlFor="crc">
                    CRC
                  </label>
                </div>
              </div>
            </div>
            <div className="form-group row">
              <div className="col-sm-10">
                <button
                  id="uploadFileBtn"
                  type="submit"
                  className="btn form-btn"
                  disabled
                >
                  Start
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>

      <div className="card">
        <div className="container">
          <h3 className="app-logo">Result</h3>
          <div id="result">
            <p>No operation done yet</p>
          </div>
        </div>
      </div>
    </>
  );
};
