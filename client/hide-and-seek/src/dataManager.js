import axios from "axios";

export const dataManager = {
  // Cryptography requests
  postEncrypt: async (formFields) => {
    const formData = new FormData();
    formData.append("file", formFields.file);
    formData.append("key", formFields.key);
    formData.append("operation", formFields.operation);
    formData.append("crc", formFields.crc);

    const response = await axios
      .post(
        `${process.env.REACT_APP_BASE_URL_BACKEND}/cryptography/encrypt`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .catch((e) => {
        return e.response;
      });
    return response;
  },

  postDecrypt: async (formFields) => {
    const formData = new FormData();
    formData.append("file", formFields.file);
    formData.append("key", formFields.key);
    formData.append("operation", formFields.operation);
    formData.append("crc", formFields.crc);

    const response = await axios
      .post(
        `${process.env.REACT_APP_BASE_URL_BACKEND}/cryptography/decrypt`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .catch((e) => {
        return e.response;
      });
    return response;
  },
};
