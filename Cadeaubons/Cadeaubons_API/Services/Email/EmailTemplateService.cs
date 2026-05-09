using Cadeaubons_Domain.DTO;

namespace Cadeaubons_API.Services.Email
{
    public static class EmailTemplateService
    {
        public static string GetVoucherGiftEmailBody(VoucherDTO dto, string voucherNumber)
        {
            return $"""
                <!doctype html>
                <html>
                  <head>
                    <meta charset="UTF-8" />
                    <title>Cadeaubon</title>
                  </head>

                  <body
                    style="
                      margin: 0;
                      padding: 0;
                      background-color: #f4f6f9;
                      font-family: Arial, sans-serif;
                    "
                  >
                    <!-- PREHEADER (hidden) -->
                    <div style="display: none; max-height: 0; overflow: hidden; opacity: 0">
                      Je hebt een nieuwe cadeaubon ontvangen 🎁
                    </div>

                    <table
                      width="100%"
                      cellpadding="0"
                      cellspacing="0"
                      border="0"
                      style="background-color: #f4f6f9; padding: 40px 0"
                    >
                      <tr>
                        <td align="center">
                          <!-- CONTAINER -->
                          <table
                            width="600"
                            align="center"
                            cellpadding="0"
                            cellspacing="0"
                            border="0"
                            style="background: #ffffff; border-collapse: collapse"
                          >
                            <!-- HEADER -->
                            <tr>
                              <td
                                style="
                                  background: #2e7d32;
                                  background-image: linear-gradient(135deg, #4caf50, #2e7d32);
                                  padding: 45px 30px;
                                  text-align: center;
                                  color: #ffffff;
                                "
                              >
                                <h1 style="margin: 0; font-size: 26px; font-weight: bold">
                                  🎁 Cadeaubon Ontvangen
                                </h1>
                              </td>
                            </tr>

                            <!-- CONTENT -->
                            <tr>
                              <td
                                style="
                                  padding: 40px 30px;
                                  font-size: 15px;
                                  color: #333333;
                                  line-height: 1.6;
                                "
                              >
                                <p style="margin: 0 0 15px 0">
                                  Beste <strong>{dto.UserFullName}</strong>,
                                </p>

                                <p style="margin: 0 0 20px 0">
                                  <strong style="color: #2e7d32"> {dto.BuyerFullName} </strong>
                                  heeft via <strong>Cadeaubon App</strong> een cadeaubon voor
                                  jou gekocht.
                                </p>

                                <!-- CARD -->
                                <table
                                  width="100%"
                                  cellpadding="0"
                                  cellspacing="0"
                                  border="0"
                                  style="
                                    background: #f8faf9;
                                    border: 1px solid #e5e7eb;
                                    border-collapse: collapse;
                                    margin: 25px 0;
                                  "
                                >
                                  <tr>
                                    <td style="padding: 20px">
                                      <h2
                                        style="
                                          margin: 0 0 15px 0;
                                          font-size: 18px;
                                          color: #2e7d32;
                                        "
                                      >
                                        Cadeaubon Details
                                      </h2>

                                      <table
                                        width="100%"
                                        cellpadding="0"
                                        cellspacing="0"
                                        border="0"
                                      >
                                        <tr>
                                          <td style="padding: 8px 0; color: #777">Nummer</td>
                                          <td
                                            align="right"
                                            style="
                                              padding: 8px 0;
                                              font-weight: bold;
                                              color: #2e7d32;
                                            "
                                          >
                                            {voucherNumber}
                                          </td>
                                        </tr>

                                        <tr>
                                          <td style="padding: 8px 0; color: #777">Thema</td>
                                          <td
                                            align="right"
                                            style="
                                              padding: 8px 0;
                                              font-weight: bold;
                                              color: #2e7d32;
                                            "
                                          >
                                            {dto.ThemeName}
                                          </td>
                                        </tr>

                                        <tr>
                                          <td style="padding: 8px 0; color: #777">Waarde</td>
                                          <td
                                            align="right"
                                            style="
                                              padding: 8px 0;
                                              font-size: 20px;
                                              font-weight: bold;
                                              color: #2e7d32;
                                            "
                                          >
                                            €{dto.InitialAmount}
                                          </td>
                                        </tr>

                                        <tr>
                                          <td style="padding: 8px 0; color: #777">
                                            Aankoopdatum
                                          </td>
                                          <td
                                            align="right"
                                            style="padding: 8px 0; font-weight: bold"
                                          >
                                            {dto.PurchaseDate:dd/MM/yyyy}
                                          </td>
                                        </tr>
                                      </table>

                                      <!-- MESSAGE -->
                                      <table
                                        width="100%"
                                        cellpadding="0"
                                        cellspacing="0"
                                        border="0"
                                        style="margin-top: 20px"
                                      >
                                        <tr>
                                          <td
                                            style="
                                              background: #e8f5e9;
                                              border-left: 4px solid #4caf50;
                                              padding: 12px;
                                              font-size: 14px;
                                            "
                                          >
                                            Je cadeaubon is onmiddellijk beschikbaar en klaar
                                            voor gebruik.
                                          </td>
                                        </tr>
                                      </table>
                                    </td>
                                  </tr>
                                </table>

                                <p style="margin: 20px 0 10px 0">Veel plezier 🎉</p>

                                <p style="margin: 0">
                                  Met vriendelijke groeten,<br />
                                  <strong>Cadeaubon App</strong>
                                </p>
                              </td>
                            </tr>

                            <!-- FOOTER -->
                            <tr>
                              <td
                                style="
                                  background: #fafafa;
                                  padding: 20px;
                                  text-align: center;
                                  font-size: 12px;
                                  color: #888888;
                                  border-top: 1px solid #eeeeee;
                                "
                              >
                                Dit is een automatische e-mail, gelieve hier niet op te
                                antwoorden.
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </body>
                </html>
                
                """;
        }
    }
}
